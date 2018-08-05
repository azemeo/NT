using System;
using System.Collections;
using System.Drawing;
using Blumind.Core;
using Blumind.Model;
using Blumind.Model.MindMaps;
using Blumind.Model.Styles;
using System.Windows.Forms;

namespace Blumind.Controls.MapViews
{
    class MindMapLayout : Layouter
    {
        protected override Rectangle Layout(Topic root, MindMapLayoutArgs e)
        {
            if (root == null)
                throw new ArgumentNullException();

            return LayoutRoot(root, e);
        }

        Rectangle LayoutRoot(Topic root, MindMapLayoutArgs args)
        {
            Size size = root.Bounds.Size; // CalculateNodeSize(root, args); // datnq TODO Update calculateNodeSize
            Point pt = new Point(0, 0);
            root.Bounds = new Rectangle(pt.X - size.Width / 2, pt.Y - size.Height / 2, size.Width, size.Height);
            var rootFullSize = LayoutAttachments(root, args);

            Vector4[] vectors = new Vector4[] { Vector4.Left, Vector4.Top, Vector4.Right };
            //int def = 0;
            //int sideCount = Math.DivRem(root.Children.Count, vectors.Length, out def);

            Rectangle allBounds = root.Bounds;
            //int subTopicIndex = 0;
            root.Lines.Clear();
            Hashtable layoutInfos = new Hashtable();
            for (int vi = 0; vi < vectors.Length; vi++)
            {
                TopicType type;
                if (vi == (int)Vector4.Left)
                {
                    type = TopicType.Threat;
                }
                else if (vi == (int)Vector4.Right)
                {
                    type = TopicType.Consequence;
                }
                else
                {
                    type = TopicType.Hazard;
                }

                XList<Topic> children = root.getChildrenByType(type);
                Topic[] subTopics = new Topic[children.Count];
                for (int ti = 0, n = children.Count; ti < n; ti++)
                {
                    subTopics[ti] = children[ti];
                }

                XList<int> rows = CalculateRow(root, subTopics, args);
                XList<int> columns = CalculateColumn(root, subTopics);
                Rectangle rectFull = LayoutSubTopics(root, subTopics, vectors[vi], rows, columns, -1, -1, args);
                if (!rectFull.IsEmpty)
                    allBounds = Rectangle.Union(allBounds, rectFull);
            }

            return allBounds;
        }

        Size CalculateTextSize(Topic topic, MindMapLayoutArgs e)
        {
            Size proposedSize = topic.TextBounds.Size;
            Font font = topic.Style.Font != null ? topic.Style.Font : e.Font;
            Size textSize;
            if (e.Graphics == null)
                textSize = TextRenderer.MeasureText(topic.Text, font, proposedSize);
            else
                textSize = Size.Ceiling(e.Graphics.MeasureString(topic.Text, font, proposedSize.Width));
            return textSize;
        }

        XList<int> CalculateRow(Topic parent, MindMapLayoutArgs e)
        {
            Topic[] empty = { };
            if (parent.Folded)
                return CalculateRow(parent, empty, e);
            else
                return CalculateRow(parent, parent.Children.ToArray(), e);
        }

        XList<int> CalculateRow(Topic parent, Topic[] children, MindMapLayoutArgs e)
        {
            XList<int> listHeight = new XList<int>();
            if (!parent.IsRoot)
            {
                parent.TextSize = CalculateTextSize(parent, e);
                listHeight.Add(parent.ContentBounds.Height);
            }
            XList<int> prevSubListHeight = new XList<int>();

            bool isIncrease = parent.IsRoot || parent.Type == TopicType.Barrier;
            int i = isIncrease ? 0 : children.Length - 1;
            while (i > - 1 && i < children.Length)
            {
                Topic subTopic = children[i];
                XList<int> subListHeight = CalculateRow(subTopic, e);
                if (subTopic.Type == TopicType.Barrier && prevSubListHeight.Count > 1)
                {
                    prevSubListHeight.RemoveAt(0);
                    if (subListHeight.IsEmpty)
                    {
                        subListHeight.AddRange(prevSubListHeight);
                    }
                    else
                    {
                        XList<int> savedList = subListHeight;
                        subListHeight = new XList<int>();
                        subListHeight.Add(savedList[0]);
                        subListHeight.AddRange(prevSubListHeight);
                        savedList.RemoveAt(0);
                        subListHeight.AddRange(savedList);
                    }
                }

                if (parent.Type == TopicType.Barrier || parent.IsRoot)
                {
                    // Append row
                    listHeight.AddRange(subListHeight);
                }
                else
                {
                    // Merge rows
                    for (int k = 0, n = listHeight.Count; k < n; ++k)
                    {
                        if (k < subListHeight.Count)
                        {
                            listHeight[k] = Math.Max(subListHeight[k], listHeight[k]);
                        }
                        else
                        {
                            break;
                        }
                    }
                    if (listHeight.Count < subListHeight.Count)
                    {
                        for (int k = listHeight.Count, n = subListHeight.Count; k < n; ++k)
                        {
                            listHeight.Add(subListHeight[k]);
                        }
                    }
                }
                prevSubListHeight = subListHeight;

                if (isIncrease)
                    ++i;
                else
                    --i;
            }
            return listHeight;
        }

        int CalculateMaxRow(Topic topic, Topic[] siblingTopics)
        {
            if (topic == null)
                return 0;

            int maxRow = 0; // Row contains itself
            bool isIncrease = !topic.IsRoot && topic.Type != TopicType.Barrier;
            int i = isIncrease ? 0 : siblingTopics.Length - 1;
            while (i > -1 && i < siblingTopics.Length)
            {
                Topic sibling = siblingTopics[i];
                int siblingRow = 0;
                if (sibling.Type == TopicType.Barrier)
                {
                    siblingRow = CalculateMaxRow(sibling.LastChild, sibling.Children.ToArray());
                }
                else
                {
                    siblingRow = CalculateMaxRow(sibling.FirstChild, sibling.Children.ToArray()) + 1;
                }
                maxRow += siblingRow;
                if (sibling == topic)
                {
                    break;
                }

                if (isIncrease)
                    ++i;
                else
                    --i;
            }
            return maxRow;
        }

        XList<int> CalculateColumn(Topic parent)
        {
            Topic[] empty = {};
            if (parent.Folded)
                return CalculateColumn(parent, empty);
            else
                return CalculateColumn(parent, parent.Children.ToArray());
        }

        XList<int> CalculateColumn(Topic parent, Topic[] children)
        {
            XList<int> listWidth = new XList<int>();
            if (parent.Type == TopicType.Barrier)
            {
                listWidth.Add(parent.ContentBounds.Width);
            }
            int index = 0;
            for (int i = 0, count = children.Length; i <= count; ++i)
            {
                Topic subTopic = null;
                XList<int> subListWidth = new XList<int>();
                if (i == count)
                {
                    if (parent.Type != TopicType.Barrier && !parent.IsRoot)
                    {
                        subTopic = parent;
                        subListWidth.Add(subTopic.ContentBounds.Width);
                    }
                    else
                        break;
                }
                else
                {
                    subTopic = children[i];
                    subListWidth = CalculateColumn(subTopic);
                }

                if (parent.Type == TopicType.Barrier)
                    index = 1;
                if (parent.IsRoot)
                    index = 0;

                for (int k = index, n = listWidth.Count; k < n; ++k)
                {
                    if (k < subListWidth.Count)
                    {
                        listWidth[k] = Math.Max(subListWidth[k], listWidth[k]);
                    }
                    else
                    {
                        break;
                    }
                }
                if (listWidth.Count < subListWidth.Count + index)
                {
                    for (int k = listWidth.Count - index, n = subListWidth.Count; k < n; ++k)
                    {
                        listWidth.Add(subListWidth[k]);
                    }
                }
                ++index;
            }
            return listWidth;
        }

        int GetYPos(int index, int space, XList<int> items)
        {
            int pos = 0;
            for (int i = 0, n = items.Count; i < n; ++i)
            {
                if (i >= index)
                    break;
                if (i >= 0)
                    pos += space;
                pos += items[i];
            }
            return pos;
        }

        int GetXPos(int index, int space, XList<int> items, Vector4 vector)
        {
            int pos = 0;
            for (int i = 0, n = items.Count; i < n; ++i)
            {
                if (vector == Vector4.Left)
                {
                    if (i > 0)
                        pos += space;
                    pos += items[i];
                }
                if (i >= index)
                    break;
                if (vector == Vector4.Right)
                {
                    if (i > 0)
                        pos += space;
                    pos += items[i];
                }
            }
            return pos;
        }

        Rectangle LayoutSubTopics(Topic parent, Topic[] subTopics, Vector4 vector, XList<int> rows, XList<int> columns, int parentRow, int parentCol, MindMapLayoutArgs e)
        {
            if (parent == null)
                throw new ArgumentNullException();

            if (rows.IsEmpty || columns.IsEmpty)
                return Rectangle.Empty;

            if (parent.Folded || parent.Children.IsEmpty)
                return Rectangle.Empty;

            int vSpace = e.ItemsSpace;
            Topic root = parent.GetRoot();

            Rectangle rectFull = Rectangle.Empty;
            Point pp = PaintHelper.CenterPoint(root.ContentBounds);

            int fullHeight = GetYPos(rows.Count - 1, vSpace, rows);
            int x = vector == Vector4.Left ? root.Left - e.LayerSpace : root.Right + e.LayerSpace;
            int y = pp.Y - (fullHeight / 2) - 30;
            int hSpace = 10;
            Topic previous = parent == root ? root : parent.ParentTopic;
            int prevRow = 0;
            for (int i = 0, n = subTopics.Length; i < n; ++i)
            {
                Topic subTopic = subTopics[i];
                subTopic.Vector = vector;
                int row = 0, column = 0;
                switch (vector)
                {
                    case Vector4.Top:
                        subTopic.Location = new Point(root.Bounds.Left - (subTopic.Width - root.Width) / 2, parent.Location.Y - e.LayerSpace - subTopic.Size.Height);
                        break;
                    case Vector4.Left:
                    case Vector4.Right:
                    default:
                        if (subTopic.Type == TopicType.Barrier)
                        {
                            column = parentCol - subTopics.Length + i;
                            row = parentRow;
                        }
                        else
                        {
                            column = parentCol + subTopic.Children.Count + 1;
                            row = parentRow + 1;
                            if (subTopic.Type == TopicType.Escalation)
                            {
                                int maxUncleRow = 0;
                                if (parent.ParentTopic != null)
                                {
                                    XList<Topic> children = parent.ParentTopic.Children;
                                    int nextIndex = children.IndexOf(parent) + 1;
                                    if (nextIndex < children.Count)
                                    {
                                        maxUncleRow = CalculateMaxRow(children[nextIndex], children.ToArray());
                                    }
                                }

                                int maxSiblingRow = 0;
                                int prevIndex = i - 1;
                                if (prevIndex >= 0)
                                {
                                    maxSiblingRow = CalculateMaxRow(parent.Children[prevIndex], parent.Children.ToArray());
                                }
                                row += maxSiblingRow + maxUncleRow;
                            }
                            else if (parent.IsRoot)
                            {
                                int prevIndex = i - 1;
                                if (prevIndex >= 0)
                                {
                                    row += CalculateMaxRow(subTopics[prevIndex], subTopics);
                                }
                            }
                        }
                        Point pt = new Point(GetXPos(column, hSpace, columns, vector), GetYPos(row, vSpace, rows));
                        if (subTopic.Type == TopicType.Barrier)
                        {
                            if (vector == Vector4.Left)
                                pt.X -= (subTopic.ContentBounds.Width - subTopic.Width) / 2;
                            else
                                pt.X += (subTopic.ContentBounds.Width - subTopic.Width) / 2;
                        }
                        subTopic.Location = new Point(vector == Vector4.Left ? x - pt.X: x + pt.X, pt.Y + y);
                        prevRow = row;
                        break;
                }

                // line
                if (!parent.Folded && subTopic.Type == TopicType.Barrier)
                {
                    var lines = CreateTopicLines(e, previous, subTopic, vector, GetReverseVector(vector));
                    if (lines != null)
                    {
                        parent.Lines.AddRange(lines);
                    }
                }
                subTopic.Lines.Clear();

                Rectangle rectFullSub = LayoutSubTopics(subTopic, subTopic.Children.ToArray(), vector, rows, columns, row, column, e);
                // Threat topic will be drawn as the last element
                if (subTopic.Type == TopicType.Threat || subTopic.Type == TopicType.Consequence || subTopic.Type == TopicType.Escalation)
                {
                    Topic beginNode, endNode;
                    if (subTopic.HasChildren && !subTopic.Folded)
                    {
                        beginNode = subTopic.Children[subTopic.Children.Count - 1];
                        endNode = subTopic;
                    }
                    else
                    {
                        beginNode = parent;
                        endNode = subTopic;
                    }
                    var lines = CreateTopicLines(e, beginNode, endNode, vector, GetReverseVector(vector));
                    if (lines != null)
                    {
                        beginNode.Lines.AddRange(lines);
                    }

                    if (subTopic.HasChildren)
                    {
                        int foldBtnSize = FoldingButtonSize;
                        if (vector == Vector4.Left)
                        {
                            subTopic.FoldingButton = new Rectangle(subTopic.Right - foldBtnSize + 4,
                                                            subTopic.Top + (int)Math.Round((60 - foldBtnSize) / 2.0f, MidpointRounding.AwayFromZero),
                                                            foldBtnSize,
                                                            foldBtnSize);
                        }
                        else if (vector == Vector4.Right)
                        {
                            subTopic.FoldingButton = new Rectangle(subTopic.Left - foldBtnSize + 4,
                                                            subTopic.Top + (int)Math.Round((60 - foldBtnSize) / 2.0f, MidpointRounding.AwayFromZero),
                                                            foldBtnSize,
                                                            foldBtnSize);
                        }
                    }
                }
                else if (subTopic.Type == TopicType.Hazard)
                {
                    var lines = CreateTopicLines(e, root, subTopic, vector, GetReverseVector(vector));
                    if (lines != null)
                    {
                        root.Lines.AddRange(lines);
                    }
                }

                rectFull = Rectangle.Union(rectFull, subTopic.ContentBounds);
                if (!rectFullSub.IsEmpty)
                    rectFull = Rectangle.Union(rectFull, rectFullSub);
                previous = subTopic;
            }

            return rectFull;
        }

        /// <summary>
        /// 计算跟节点下的各个子节点之间的合适距离
        /// </summary>
        /// <param name="subTopics">子节点集合</param>
        /// <param name="layerSpace"></param>
        /// <param name="minSpace">最小距离</param>
        /// <returns></returns>
        int GetRootItemsSpace(Topic parent, Topic[] subTopics, Hashtable layoutInfos, MindMapLayoutArgs e)
        {
            if (subTopics == null || subTopics.Length == 0)
                return 0;

            int layerSpace = e.LayerSpace;

            int count = subTopics.Length;
            double angle = Math.PI / (count + 1) * (count - 1);
            int c1 = (int)Math.Ceiling(Math.Sin(angle / 2) * layerSpace * 2);

            for (int i = 0; i < count; i++)
            {
                int h = subTopics[i].Height;// ((TopicLayoutInfo)layoutInfos[subTopics[i]]).FullSize.Height;
                if (i == 0 || i == count - 1)
                    c1 -= h / 2;
                else
                    c1 -= h;
            }

            return Math.Max(c1, e.ItemsSpace);
        }

        protected override TopicLine CreateTopicLine(MindMapLayoutArgs e, Topic beginTopic, Topic endTopic, Vector4 beginSide, Vector4 endSide)
        {
            var beginRect = beginTopic.Bounds;
            var endRect = endTopic.Bounds;

            if (e.ShowLineArrowCap)
            {
                endRect.Inflate(LineAnchorSize, LineAnchorSize);
            }

            if (beginTopic != null && !beginTopic.IsRoot)
            {
                int foldBtnSize = endTopic.FoldingButton.Width;
                switch (beginSide)
                {
                    case Vector4.Left:
                        //beginRect.X -= foldBtnSize;
                        //beginRect.Width += foldBtnSize;
                        break;
                    case Vector4.Right:
                        // beginRect.Width += foldBtnSize;
                        break;
                    case Vector4.Top:
                        beginRect.Y -= foldBtnSize;
                        beginRect.Height += foldBtnSize;
                        break;
                    case Vector4.Bottom:
                        beginRect.Height += foldBtnSize;
                        break;
                }
            }

            if (beginTopic != null && endTopic != null &&
                beginTopic.Type == TopicType.Barrier &&
                beginTopic.IsParentOf(endTopic))
            {
                beginRect.Y = beginTopic.ContentBounds.Bottom;
                beginRect.Height = 0;
            }

            if (endTopic != null)
            {
                if (endTopic.Type == TopicType.Threat ||
                    endTopic.Type == TopicType.Consequence ||
                    endTopic.Type == TopicType.Escalation)
                {
                    endRect.Height = 60; // 60 is barrier height
                }
                else if (endTopic.Style.Shape == TopicShape.BaseLine)
                {
                    endRect.Y = endRect.Bottom;
                    endRect.Height = 0;
                }
            }

            return new TopicLine(endTopic, beginSide, endSide, beginRect, endRect);
        }

        protected override XList<TopicLine> CreateTopicLines(MindMapLayoutArgs e, Topic beginTopic, Topic endTopic, Vector4 beginSide, Vector4 endSide)
        {
            XList<TopicLine> lines = new XList<TopicLine>();
            var beginRect = beginTopic.Bounds;
            var endRect = endTopic.Bounds;

            if (e.ShowLineArrowCap)
            {
                endRect.Inflate(LineAnchorSize, LineAnchorSize);
            }

            if (beginTopic != null && !beginTopic.IsRoot)
            {
                int foldBtnSize = endTopic.FoldingButton.Width;
                switch (beginSide)
                {
                    case Vector4.Left:
                        break;
                    case Vector4.Right:
                        break;
                    case Vector4.Top:
                        beginRect.Y -= foldBtnSize;
                        beginRect.Height += foldBtnSize;
                        break;
                    case Vector4.Bottom:
                        beginRect.Height += foldBtnSize;
                        break;
                }
            }

            if (beginTopic != null && endTopic != null &&
                beginTopic.Type == TopicType.Barrier &&
                beginTopic.IsParentOf(endTopic))
            {
                beginRect.Y = beginTopic.ContentBounds.Bottom;
                beginRect.Height = 0;
            }

            if (endTopic != null)
            {
                if (endTopic.Type == TopicType.Threat ||
                    endTopic.Type == TopicType.Consequence ||
                    endTopic.Type == TopicType.Escalation)
                {
                    endRect.Height = 60; // 60 is barrier height
                }
                else if (endTopic.Style.Shape == TopicShape.BaseLine)
                {
                    endRect.Y = endRect.Bottom;
                    endRect.Height = 0;
                }
            }

            TopicLine line;
            if (endTopic != null && beginTopic != null && endTopic.Type == TopicType.Barrier && beginTopic.IsRoot)
            {
                endRect = endTopic.ContentBounds;
                endRect.Height = endTopic.Bounds.Height;
                line = new TopicLine(endTopic, beginSide, endSide, beginRect, endRect);
                lines.Add(line);

                beginRect = endRect;
                int left = beginRect.Left;
                if (beginSide == Vector4.Left)
                {
                    left += beginRect.Width;
                }
                else if (beginSide == Vector4.Right)
                {
                    left -= beginRect.Width;
                }

                beginRect.Location = new Point(left, beginRect.Y);
                endRect = endTopic.Bounds;
            }
            line = new TopicLine(endTopic, beginSide, endSide, beginRect, endRect);
            lines.Add(line);
            return lines;
        }

        //public override void AdjustLineRect(TopicLine line, Topic fromTopic, ref Rectangle rectFrom, ref Rectangle rectTo)
        //{
        //    base.AdjustLineRect(line, fromTopic, ref rectFrom, ref rectTo);

        //    if (fromTopic != null && !fromTopic.IsRoot)
        //    {
        //        int foldBtnSize = line.Target.FoldingButton.Width;
        //        switch (line.BeginSide)
        //        {
        //            case Vector4.Left:
        //                rectFrom.X -= foldBtnSize;
        //                rectFrom.Width += foldBtnSize;
        //                break;
        //            case Vector4.Right:
        //                rectFrom.Width += foldBtnSize;
        //                break;
        //            case Vector4.Top:
        //                rectFrom.Y -= foldBtnSize;
        //                rectFrom.Height += foldBtnSize;
        //                break;
        //            case Vector4.Bottom:
        //                rectFrom.Height += foldBtnSize;
        //                break;
        //        }
        //    }

        //    if (line.Target != null && line.Target.Style.Shape == Core.Styles.TopicShape.BaseLine)
        //    {
        //        rectTo.Y = rectTo.Bottom;
        //        rectTo.Height = 0;
        //    }
        //}

        public override Topic GetNextNode(Topic from, MoveVector vector)
        {
            Topic next = null;

            if (from.IsRoot)
            {
                switch (vector)
                {
                    case MoveVector.Left:
                    case MoveVector.Up:
                        foreach (Topic subTopic in from.Children)
                        {
                            if (subTopic.Vector == Vector4.Left)
                            {
                                next = subTopic;
                                break;
                            }
                        }
                        break;
                    case MoveVector.Right:
                    case MoveVector.Down:
                        foreach (Topic subTopic in from.Children)
                        {
                            if (subTopic.Vector == Vector4.Right)
                            {
                                next = subTopic;
                                break;
                            }
                        }
                        break;
                }
            }
            else
            {
                switch (vector)
                {
                    case MoveVector.Left:
                        if (from.Vector == Vector4.Left)
                            next = from.HasChildren ? from.Children[0] : null;
                        else
                            next = from.ParentTopic;
                        break;
                    case MoveVector.Right:
                        if (from.Vector == Vector4.Right)
                            next = from.HasChildren ? from.Children[0] : null;
                        else
                            next = from.ParentTopic;
                        break;
                    case MoveVector.Up:
                        next = from.GetSibling(false, false, true);
                        break;
                    case MoveVector.Down:
                        next = from.GetSibling(true, false, true);
                        break;
                }
            }

            return next;
        }
    }
}
