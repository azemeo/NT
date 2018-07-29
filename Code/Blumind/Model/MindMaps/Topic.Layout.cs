using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using Blumind.Configuration;
using Blumind.Core;
using Blumind.Globalization;

namespace Blumind.Model.MindMaps
{
    partial class Topic
    {
        int? _CustomWidth;
        int? _CustomHeight;
        //Rectangle _TextBounds;

        [DefaultValue(null), LocalDisplayName("Custom Width"), LocalCategory("Layout")]
        public int? CustomWidth
        {
            get { return _CustomWidth; }
            set
            {
                if (_CustomWidth != value)
                {
                    _CustomWidth = value;
                    OnWidthChanged();
                }
            }
        }

        [DefaultValue(null), LocalDisplayName("Custom Height"), LocalCategory("Layout")]
        public int? CustomHeight
        {
            get { return _CustomHeight; }
            set
            {
                if (_CustomHeight != value)
                {
                    _CustomHeight = value;
                    OnHeightChanged();
                }
            }
        }

        public static Size getMinTextSize(TopicType type)
        {
            switch (type)
            {
                case TopicType.TopEvent:
                    return new Size(80, 80);
                case TopicType.Hazard:
                    return new Size(150, 75);
                case TopicType.Barrier:
                    return new Size(160, 50);
                case TopicType.Consequence:
                case TopicType.Threat:
                case TopicType.Escalation:
                    return new Size(150, 100);
                default:
                    return new Size(100, 100);
            }
        }

        [Browsable(false)]
        public Rectangle TextBounds { get; set; }

        [Browsable(false)]
        public Size TextSize
        {
            get
            {
                return TextBounds.Size;
            }

            set
            {
                Size oldSize = TextBounds.Size;
                Size minSize = getMinTextSize(Type);
                if (oldSize.Width != value.Width || oldSize.Height != value.Height)
                {
                    if (value.Width < minSize.Width)
                        value.Width = minSize.Width;
                    if (value.Height < minSize.Height)
                        value.Height = minSize.Height;

                    TextBounds = new Rectangle(TextBounds.Location, value);

                    // Update bounds if need
                    if (Type != TopicType.Barrier)
                    {
                        Size boundSize = Bounds.Size;
                        if (TextBounds.Width + 10 != boundSize.Width)
                        {
                            boundSize.Width = TextBounds.Width + 10;
                        }
                        if (Type == TopicType.Hazard)
                        {
                            if (TextBounds.Height + 35 != Height)
                            {
                                boundSize.Height = TextBounds.Height + 35;
                            }
                        }
                        else
                        {
                            if (TextBounds.Height + 10 != Height)
                            {
                                boundSize.Height = TextBounds.Height + 10;
                            }
                        }
                        Bounds = new Rectangle(Bounds.Location, boundSize);
                    }
                }
            }
        }

        [Browsable(false)]
        public Rectangle GlobalTextBounds
        {
            get
            {
                Rectangle globalTextBounds = TextBounds;
                globalTextBounds.X += Bounds.X;
                globalTextBounds.Y += Bounds.Y;
                return globalTextBounds;
            }
        }

        /// <summary>
        /// 已经经过坐标转换
        /// 相对 Topic 左上角
        /// </summary>
        [Browsable(false)]
        public Rectangle RemarkIconBounds
        {
            get;
            set;
            //get
            //{
            //    const int IconSize = 16;
            //    const int Space = 2;

            //    return new Rectangle(0,
            //        Height + Space,
            //        IconSize,
            //        IconSize);
            //}
        }

        [Browsable(false)]
        public Rectangle FullBounds
        {
            get
            {
                var rect = Bounds;
                if (this.HaveRemark && Options.Current.GetBool(Blumind.Configuration.OptionNames.Charts.ShowRemarkIcon))
                {
                    rect = Rectangle.Union(rect, RemarkIconBounds);
                }
                Rectangle globalTextBounds = TextBounds;
                globalTextBounds.X += Bounds.X;
                globalTextBounds.Y += Bounds.Y;
                rect = Rectangle.Union(globalTextBounds, Bounds);
                return rect;
            }
        }

        [Browsable(false)]
        public Rectangle ContentBounds
        {
            get
            {
                var rect = Bounds;
                Rectangle globalTextBounds = TextBounds;
                globalTextBounds.X += Bounds.X;
                globalTextBounds.Y += Bounds.Y;
                rect = Rectangle.Union(globalTextBounds, Bounds);
                return rect;
            }
        }

        [Browsable(false)]
        public Rectangle FoldingButton { get; set; }

        [Browsable(false)]
        public bool FoldingButtonVisible { get; set; }
    }
}
