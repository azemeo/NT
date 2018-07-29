using System;
using System.Collections;
using System.Drawing;
using Blumind.Core;
using Blumind.Model;
using Blumind.Model.MindMaps;
using Blumind.Model.Styles;

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

                Size fullSize = CalculateSizes(root, rootFullSize, subTopics, args, vectors[vi], layoutInfos);
                Rectangle rectFull = LayoutSubTopics(root, subTopics, vectors[vi], layoutInfos, args);
                if (!rectFull.IsEmpty)
                    allBounds = Rectangle.Union(allBounds, rectFull);
            }

            return allBounds;
        }

        Size CalculateSizes(Topic parent, Size parentFullSize, Topic[] subTopics, MindMapLayoutArgs e, Vector4 vector, Hashtable layoutInfos)
        {
            Size fullSize = parentFullSize;// parent.Size;

            if (subTopics != null && subTopics.Length > 0)
            {
                Topic previous = null;
                int previousHeight = 0;
                foreach (Topic subTopic in subTopics)
                {
                    // subTopic.Size = CalculateNodeSize(subTopic, e); // datnq
                    var subTopicFullSize = LayoutAttachments(subTopic, e);
                    Size subSize = CalculateSizes(subTopic, subTopicFullSize, e, vector, layoutInfos);
                    if (parent.Type == TopicType.Barrier)
                    {
                        fullSize.Height += subSize.Height + e.ItemsSpace;
                    }
                    else if (subTopic.Type == TopicType.Barrier && subTopic.HasChildren)
                    {
                        if (previous != null && previous.Type == TopicType.Barrier && previous.HasChildren)
                        {
                            subSize.Height += previousHeight;
                        }
                        fullSize.Height = Math.Max(fullSize.Height, subSize.Height + e.ItemsSpace);
                        previousHeight = subSize.Height - subTopic.ContentBounds.Height - e.ItemsSpace;
                    }

                    if (previous != null)
                    {
                        fullSize.Width = Math.Max(fullSize.Width, subSize.Width);
                    }
                    previous = subTopic;
                }

                fullSize.Width += e.LayerSpace + parent.Size.Width / 2;
            }

            layoutInfos[parent] = new TopicLayoutInfo(fullSize);
            return fullSize;
        }

        Size CalculateSizes(Topic parent, Size parentFullSize, MindMapLayoutArgs e, Vector4 vector, Hashtable layoutInfos)
        {
            if (parent.Folded)
                return CalculateSizes(parent, parentFullSize, null, e, vector, layoutInfos);
            else
                return CalculateSizes(parent, parentFullSize, parent.Children.ToArray(), e, vector, layoutInfos);
        }

        int nextBarrierHeight(Topic topic, Hashtable layoutInfos)
        {
            int height = 0;
            if (topic.Type == TopicType.Barrier)
            {
                Topic next = topic.NextSibling;
                if (next != null && next.HasChildren)
                {
                    height += ((TopicLayoutInfo)layoutInfos[next]).FullSize.Height - next.ContentBounds.Height + nextBarrierHeight(next, layoutInfos);
                }
            }
            return height;
        }

        Rectangle LayoutSubTopics(Topic parent, Topic[] subTopics, Vector4 vector, Hashtable layoutInfos, MindMapLayoutArgs e)
        {
            if (parent == null)
                throw new ArgumentNullException();

            if (!layoutInfos.Contains(parent))
                return Rectangle.Empty;

            if (parent.Folded || parent.Children.IsEmpty)
                return Rectangle.Empty;

            int nodeSpace = e.ItemsSpace;
            if (parent.IsRoot)
            {
                nodeSpace = GetRootItemsSpace(parent, subTopics, layoutInfos, e);
            }

            Topic root = parent.GetRoot();

            // get full height
            int fullHeight = 0;
            
            for (int i = 0; i < subTopics.Length; i++)
            {
                if (i > 0)
                    fullHeight += nodeSpace;
                fullHeight += ((TopicLayoutInfo)layoutInfos[subTopics[i]]).FullSize.Height;
            }

            //
            Rectangle rectFull = Rectangle.Empty;
            int y = 0;
            if (parent.Type == TopicType.Barrier)
            {
                y = parent.ContentBounds.Bottom + nodeSpace + nextBarrierHeight(parent, layoutInfos);
            }
            else
            {
                Point pp = PaintHelper.CenterPoint(parent.ContentBounds);
                y = pp.Y - (fullHeight / 2);
            }
            int space = e.LayerSpace;
            int barrierSpace = 10;
            Topic previous = parent == root ? root : parent.ParentTopic;
            foreach (var subTopic in subTopics)
            {
                var subTif = (TopicLayoutInfo)layoutInfos[subTopic];
                subTopic.Vector = vector;
                Point pt;
                switch (vector)
                {
                    case Vector4.Top:
                        pt = new Point(root.Bounds.Left - (subTopic.Width - root.Width) / 2, parent.Location.Y - space - subTopic.Size.Height / 2);
                        break;
                    case Vector4.Left:
                        if (parent.Type == TopicType.Threat || parent.Type == TopicType.Escalation)
                        {
                            int localX = 0;
                            if (previous == root)
                            {
                                localX = previous.Bounds.Left - space - (subTopic.ContentBounds.Width + subTopic.Bounds.Width) / 2;
                            }
                            else
                            {
                                localX = previous.Bounds.Left - barrierSpace - subTopic.ContentBounds.Width;
                            }
                            pt = new Point(localX, parent.Location.Y);
                        }
                        else
                        {
                            pt = new Point(parent.Bounds.Left - space - subTopic.Bounds.Width, y);
                        }
                        break;
                    case Vector4.Right:
                    default:
                        if (parent.Type == TopicType.Consequence || parent.Type == TopicType.Escalation)
                        {
                            int localX = 0;
                            if (previous == root)
                            {
                                localX = previous.ContentBounds.Right + space + (subTopic.ContentBounds.Width - subTopic.Bounds.Width) / 2;
                            }
                            else
                            {
                                localX = previous.ContentBounds.Right + barrierSpace + (subTopic.TextBounds.Width - subTopic.Bounds.Width) / 2;
                            }
                            pt = new Point(localX, parent.Location.Y);
                        }
                        else
                        {
                            pt = new Point(parent.Bounds.Right + space, y);
                        }
                        break;
                }
                subTopic.Location = new Point(pt.X, pt.Y);

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

                Rectangle rectFullSub = LayoutSubTopics(subTopic, subTopic.Children.ToArray(), vector, layoutInfos, e);
                // Threat topic will be drawn as the last element
                if (subTopic.Type == TopicType.Threat || subTopic.Type == TopicType.Consequence || subTopic.Type == TopicType.Escalation)
                {
                    Topic beginNode, endNode;
                    if (subTopic.HasChildren && !subTopic.Folded)
                    {
                        Topic lastChild = subTopic.Children[subTopic.Children.Count - 1];
                        if (vector == Vector4.Left)
                        {
                            subTopic.Location = new Point(lastChild.ContentBounds.Left - barrierSpace - lastChild.ContentBounds.Width, subTopic.Location.Y);
                        }
                        else if (vector == Vector4.Right)
                        {
                            subTopic.Location = new Point(lastChild.ContentBounds.Right + barrierSpace, subTopic.Location.Y);
                        }
                        beginNode = lastChild;
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

                y += subTif.FullSize.Height + nodeSpace;
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
