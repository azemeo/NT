﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Blumind.Controls;
using Blumind.Controls.MapViews;
using Blumind.Core;
using Blumind.Design;
using Blumind.Dialogs;
using Blumind.Globalization;
using Blumind.Model;
using Blumind.Model.MindMaps;
using Blumind.Model.Styles;

namespace Blumind.ChartPageView
{
    class MindMapChartPage : BaseChartPage, IThemableUI
    {
        MindMapView mindMapView1;

        public MindMapChartPage()
        {
            InitializeComponent();

            UITheme.Default.Listeners.Add(this);
            ApplyTheme(UITheme.Default);
        }

        [Browsable(false)]
        public MindMap MindMapChart
        {
            get { return mindMapView1.Map; }
        }

        [Browsable(false)]
        public override ChartControl ChartBox
        {
            get
            {
                return mindMapView1;
            }
        }

        void InitializeComponent()
        {
            mindMapView1 = new Blumind.Controls.MapViews.MindMapView();
            SuspendLayout();

            // mindMapView1
            mindMapView1.Dock = DockStyle.Fill;
            mindMapView1.Name = "mindMapView1";
            mindMapView1.ShowBorder = false;
            mindMapView1.SelectionChanged += new System.EventHandler(this.mindMapView1_SelectionChanged);
            mindMapView1.ChartBackColorChanged += new System.EventHandler(this.mindMapView1_ChartBackColorChanged);

            // MindMapChartPage
            Controls.Add(this.mindMapView1);
            Name = "MindMapChartPage";
            ResumeLayout(false);
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();

            if (!this.DesignMode)
                BackColor = PaintHelper.WithoutAlpha(mindMapView1.ChartBackColor);
        }

        protected override void OnChartChanged()
        {
            base.OnChartChanged();

            if (Chart is MindMap)
            {
                MindMap chart = (MindMap)Chart;
                mindMapView1.Map = chart;
                chart.NameChanged += new EventHandler(chart_NameChanged);
                chart.LayoutTypeChanged += new EventHandler(chart_LayoutTypeChanged);
            }
            else
            {
                throw new ArgumentNullException();
            }

            Icon = GetIconByLayoutType();
        }

        void chart_LayoutTypeChanged(object sender, EventArgs e)
        {
            Icon = GetIconByLayoutType();
        }

        Image GetIconByLayoutType()
        {
            if (MindMapChart == null)
                return null;

            return MindMapLayoutTypeEditor.GetIcon(MindMapChart.LayoutType);
        }

        void chart_NameChanged(object sender, EventArgs e)
        {
            if(Chart != null)
                Text = Chart.Name;
        }

        void mindMapView1_ChartBackColorChanged(object sender, EventArgs e)
        {
            BackColor = PaintHelper.WithoutAlpha(mindMapView1.ChartBackColor);
        }

        protected override void OnSelectedObjectsChanged()
        {
            //if (SelectedObjects != null && SelectedObjects.Length == 1 && SelectedObjects[0] is Topic)
            //{
            //    // mapView1.Select((Topic)SelectedObjects);
            //    TsbFormatPainter.Enabled = true;
            //}
            //else
            //{
            //    TsbFormatPainter.Enabled = TsbFormatPainter.Checked;
            //}

            ShowProperty(SelectedObjects);

            ResetControlStatus();

            base.OnSelectedObjectsChanged();
        }

        void ShowProperty(object[] SelectedObjects)
        {
            //throw new NotImplementedException();
        }

        protected override void ResetControlStatus()
        {
            base.ResetControlStatus();
        }

        void mindMapView1_SelectionChanged(object sender, EventArgs e)
        {
            var so = mindMapView1.SelectedObjects;
            if (so == null || so.Length == 0)
                SelectedObjects = new object[] { MindMapChart };
            else
                SelectedObjects = so;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (MindMapChart != null && MindMapChart.Root != null)
            {
                mindMapView1.EnsureVisible(MindMapChart.Root);
            }
        }

        protected override void OnKeyMapChanged()
        {
            MenuAddThreat.ShortcutKeyDisplayString = KeyMap.AddThreat.ToString();
            MenuAddConsequence.ShortcutKeyDisplayString = KeyMap.AddConsequence.ToString();
            MenuAddBarrier.ShortcutKeyDisplayString = KeyMap.AddBarrier.ToString();
            MenuAddEscalation.ShortcutKeyDisplayString = KeyMap.AddEscalation.ToString();

            MenuCollapseFolding.ShortcutKeyDisplayString = KeyMap.Collapse.ToString();
            MenuExpandFolding.ShortcutKeyDisplayString = KeyMap.Expand.ToString();
            MenuToggleFolding.ShortcutKeyDisplayString = KeyMap.ToggleFolding.ToString();
            MenuCollapseAll.ShortcutKeyDisplayString = KeyMap.CollapseAll.ToString();
            MenuExpandAll.ShortcutKeyDisplayString = KeyMap.ExpandAll.ToString();

            MenuCut.ShortcutKeyDisplayString = KeyMap.Cut.ToString();
            MenuCopy.ShortcutKeyDisplayString = KeyMap.Copy.ToString();
            MenuPaste.ShortcutKeyDisplayString = KeyMap.Paste.ToString();
            MenuDelete.ShortcutKeyDisplayString = KeyMap.Delete.ToString();

            MenuEdit.ShortcutKeyDisplayString = KeyMap.Edit.ToString();
        }

        protected override void OnCurrentLanguageChanged()
        {
            if (!Created)
                return;

            MenuOpenHyperlink.Text = Lang._("Open Hyperlink");
            MenuAddThreat.Text = Lang._("Add Threat");
            MenuAddConsequence.Text = Lang._("Add Consequence");
            MenuAddBarrier.Text = Lang._("Add Barrier");
            MenuAddBarrierLeft.Text = Lang._("Add Barrier Left");
            MenuAddBarrierRight.Text = Lang._("Add Barrier Right");
            MenuAddEscalation.Text = Lang._("Add Escalation");
            MenuAdd.Text = Lang._("Add");
            MenuAddIcon.Text = Lang.GetTextWithEllipsis("Icon");
            MenuAddProgressBar.Text = Lang.GetTextWithEllipsis("Progress Bar");
            MenuAddRemark.Text = Lang.GetTextWithEllipsis("Notes");
            MenuFolding.Text = Lang._("Folding");
            MenuCollapseFolding.Text = Lang._("Collapse");
            MenuExpandFolding.Text = Lang._("Expand");
            MenuToggleFolding.Text = Lang._("Toggle Folding");
            MenuCollapseAll.Text = Lang._("Collapse All");
            MenuExpandAll.Text = Lang._("Expand All");
            MenuCut.Text = Lang._("Cut");
            MenuCopy.Text = Lang._("Copy");
            MenuPaste.Text = Lang._("Paste");
            MenuDelete.Text = Lang._("Delete");
            MenuEdit.Text = Lang._("Edit");
            MenuProperty.Text = Lang._("Property");
            MenuLink.Text = Lang._("Link");
            MenuStraightening.Text = Lang._("Straightening");
            MenuInvert.Text = Lang._("Invert");
            MenuAdvance.Text = Lang._("Advance");
            MenuNewChartFromHere.Text = Lang._("New Chart From Here");

            if (MenuLayout != null)
            {
                MenuLayout.Dispose();
                MenuLayout = null;
            }
        }

        #region menus
        ToolStripMenuItem MenuOpenHyperlink;
        ToolStripSeparator toolStripSeparator15;
        ToolStripMenuItem MenuAddThreat;
        ToolStripMenuItem MenuAddConsequence;
        ToolStripMenuItem MenuAddBarrier;
        ToolStripMenuItem MenuAddBarrierLeft;
        ToolStripMenuItem MenuAddBarrierRight;
        ToolStripMenuItem MenuAddEscalation;
        ToolStripMenuItem MenuAdd;
        ToolStripMenuItem MenuAddIcon;
        ToolStripMenuItem MenuAddProgressBar;
        ToolStripMenuItem MenuAddRemark;
        ToolStripSeparator toolStripSeparator5;
        ToolStripMenuItem MenuLink;
        ToolStripMenuItem MenuStraightening;
        ToolStripMenuItem MenuInvert;
        ToolStripMenuItem MenuFolding;
        ToolStripMenuItem MenuCollapseFolding;
        ToolStripMenuItem MenuExpandFolding;
        ToolStripMenuItem MenuToggleFolding;
        ToolStripSeparator toolStripSeparator9;
        ToolStripMenuItem MenuCollapseAll;
        ToolStripMenuItem MenuExpandAll;
        ToolStripSeparator toolStripMenuItem2;
        ToolStripMenuItem MenuCut;
        ToolStripMenuItem MenuCopy;
        ToolStripMenuItem MenuPaste;
        ToolStripMenuItem MenuDelete;
        ToolStripSeparator toolStripSeparator10;
        ToolStripMenuItem MenuEdit;
        ToolStripMenuItem MenuProperty;
        ToolStripMenuItem MenuAdvance;
        ToolStripMenuItem MenuNewChartFromHere;

        protected override int ExtendActionMenuIndex
        {
            get
            {
                return ChartContextMenuStrip.Items.IndexOf(toolStripSeparator10);
            }
        }

        protected override void InitializationChartContextMenuStrip(ContextMenuStrip contextMenu)
        {
            base.InitializationChartContextMenuStrip(contextMenu);
            contextMenu.SuspendLayout();

            //
            MenuOpenHyperlink = new ToolStripMenuItem();
            toolStripSeparator15 = new ToolStripSeparator();
            MenuAddThreat = new ToolStripMenuItem();
            MenuAddConsequence = new ToolStripMenuItem();
            MenuAddBarrier = new ToolStripMenuItem();
            MenuAddBarrierLeft = new ToolStripMenuItem();
            MenuAddBarrierRight = new ToolStripMenuItem();
            MenuAddEscalation = new ToolStripMenuItem();
            MenuAdd = new ToolStripMenuItem();
            MenuAddIcon = new ToolStripMenuItem();
            MenuAddProgressBar = new ToolStripMenuItem();
            MenuAddRemark = new ToolStripMenuItem();
            toolStripSeparator5 = new ToolStripSeparator();
            MenuLink = new ToolStripMenuItem();
            MenuStraightening = new ToolStripMenuItem();
            MenuInvert = new ToolStripMenuItem();
            MenuFolding = new ToolStripMenuItem();
            MenuCollapseFolding = new ToolStripMenuItem();
            MenuExpandFolding = new ToolStripMenuItem();
            MenuToggleFolding = new ToolStripMenuItem();
            toolStripSeparator9 = new ToolStripSeparator();
            MenuCollapseAll = new ToolStripMenuItem();
            MenuExpandAll = new ToolStripMenuItem();
            toolStripMenuItem2 = new ToolStripSeparator();
            MenuCut = new ToolStripMenuItem();
            MenuCopy = new ToolStripMenuItem();
            MenuPaste = new ToolStripMenuItem();
            MenuDelete = new ToolStripMenuItem();
            toolStripSeparator10 = new ToolStripSeparator();
            MenuAdvance = new ToolStripMenuItem();
            MenuEdit = new ToolStripMenuItem();
            MenuProperty = new ToolStripMenuItem();
            MenuNewChartFromHere = new ToolStripMenuItem();

            //
            contextMenu.Items.AddRange(new ToolStripItem[] {
                MenuOpenHyperlink,
                toolStripSeparator15,
                MenuAddThreat,
                MenuAddConsequence,
                MenuAddBarrier,
                MenuAddBarrierLeft,
                MenuAddBarrierRight,
                MenuAddEscalation,
                //MenuAdd,
                toolStripSeparator5,
                MenuLink,
                MenuFolding,
                //MenuAdvance,
                toolStripMenuItem2,
                MenuCut,
                MenuCopy,
                MenuPaste,
                MenuDelete,
                toolStripSeparator10,
                MenuEdit,
                //MenuProperty
            });

            // MenuOpenHyperlink
            MenuOpenHyperlink.Image = Blumind.Properties.Resources.hyperlink;
            MenuOpenHyperlink.Name = "MenuOpenHyperlink";
            MenuOpenHyperlink.Text = "&Open Hyperlink";
            MenuOpenHyperlink.Click += new System.EventHandler(MenuOpenHyperlink_Click);

            // toolStripSeparator15
            toolStripSeparator15.Name = "toolStripSeparator15";

            // MenuAddThreat
            MenuAddThreat.Image = Blumind.Properties.Resources.threat;
            MenuAddThreat.Name = "MenuAddThreat";
            MenuAddThreat.ShortcutKeyDisplayString = "Enter";
            MenuAddThreat.Text = "Add Threat";
            MenuAddThreat.Click += new System.EventHandler(MenuAddThreat_Click);

            // MenuAddConsequence
            MenuAddConsequence.Image = Blumind.Properties.Resources.consequence;
            MenuAddConsequence.Name = "MenuAddConsequence";
            MenuAddConsequence.ShortcutKeyDisplayString = "Enter";
            MenuAddConsequence.Text = "Add Consequence";
            MenuAddConsequence.Click += new System.EventHandler(MenuAddConsequence_Click);

            // MenuAddBarrier
            MenuAddBarrier.Image = Blumind.Properties.Resources.barrier;
            MenuAddBarrier.Name = "MenuAddBarrier";
            MenuAddBarrier.ShortcutKeyDisplayString = "Tab";
            MenuAddBarrier.Text = "Add Barrier";
            MenuAddBarrier.Click += new System.EventHandler(MenuAddBarrier_Click);

            // MenuAddBarrierLeft
            MenuAddBarrierLeft.Image = Blumind.Properties.Resources.barrier_left;
            MenuAddBarrierLeft.Name = "MenuAddBarrierLeft";
            // MenuAddBarrierLeft.ShortcutKeyDisplayString = "Tab";
            MenuAddBarrierLeft.Text = "Add Barrier Left";
            MenuAddBarrierLeft.Click += new System.EventHandler(MenuAddBarrierLeft_Click);

            // MenuAddBarrierRight
            MenuAddBarrierRight.Image = Blumind.Properties.Resources.barrier_right;
            MenuAddBarrierRight.Name = "MenuAddBarrierRight";
            // MenuAddBarrierRight.ShortcutKeyDisplayString = "Tab";
            MenuAddBarrierRight.Text = "Add Barrier Right";
            MenuAddBarrierRight.Click += new System.EventHandler(MenuAddBarrierRight_Click);

            // MenuAddEscalation
            MenuAddEscalation.Image = Blumind.Properties.Resources.escalation;
            MenuAddEscalation.Name = "MenuAddEscalation";
            MenuAddEscalation.ShortcutKeyDisplayString = "Insert";
            MenuAddEscalation.Text = "Add Escalation";
            MenuAddEscalation.Click += new System.EventHandler(MenuAddEscalationFactor_Click);

            // MenuAdd
            MenuAdd.DropDownItems.AddRange(new ToolStripItem[] {
                MenuAddIcon,
                MenuAddProgressBar,
                MenuAddRemark});
            MenuAdd.Name = "MenuAdd";
            MenuAdd.Text = "Add";

            // MenuAddIcon
            MenuAddIcon.Image = Blumind.Properties.Resources.image;
            MenuAddIcon.Name = "MenuAddIcon";
            MenuAddIcon.Text = "&Icon";
            MenuAddIcon.Click += new System.EventHandler(MenuAddIcon_Click);

            // MenuAddProgressBar
            MenuAddProgressBar.Image = Blumind.Properties.Resources.progress_bar;
            MenuAddProgressBar.Name = "MenuAddProgressBar";
            MenuAddProgressBar.Text = "&Progress Bar";
            MenuAddProgressBar.Click += new System.EventHandler(MenuAddProgressBar_Click);

            // MenuAddRemark
            MenuAddRemark.Image = Blumind.Properties.Resources.notes;
            MenuAddRemark.Name = "MenuAddRemark";
            MenuAddRemark.Text = "&Notes";
            MenuAddRemark.Click += new System.EventHandler(MenuAddRemark_Click);

            // toolStripSeparator5
            toolStripSeparator5.Name = "toolStripSeparator5";

            // MenuLink
            MenuLink.DropDownItems.AddRange(new ToolStripItem[] {
                MenuStraightening,
                MenuInvert});
            MenuLink.Name = "MenuLink";
            MenuLink.Text = "Link";

            // MenuStraightening
            MenuStraightening.Name = "MenuStraightening";
            MenuStraightening.Text = "Straightening";
            MenuStraightening.Click += new System.EventHandler(MenuStraightening_Click);

            // MenuInvert
            MenuInvert.Name = "MenuInvert";
            MenuInvert.Text = "Invert";
            MenuInvert.Click += new System.EventHandler(MenuInvert_Click);

            // MenuFolding
            MenuFolding.DropDownItems.AddRange(new ToolStripItem[] {
                MenuCollapseFolding,
                MenuExpandFolding,
                MenuToggleFolding,
                toolStripSeparator9,
                MenuCollapseAll,
                MenuExpandAll});
            MenuFolding.Name = "MenuFolding";
            MenuFolding.Text = "&Folding";

            // MenuCollapseFolding
            MenuCollapseFolding.Name = "MenuCollapseFolding";
            MenuCollapseFolding.ShortcutKeyDisplayString = "Shift+-";
            MenuCollapseFolding.Text = "Collapse";
            MenuCollapseFolding.Click += new System.EventHandler(MenuCollapseFolding_Click);

            // MenuExpandFolding
            MenuExpandFolding.Name = "MenuExpandFolding";
            MenuExpandFolding.ShortcutKeyDisplayString = "Shift++";
            MenuExpandFolding.Text = "Expand";
            MenuExpandFolding.Click += new System.EventHandler(MenuExpandFolding_Click);

            // MenuToggleFolding
            MenuToggleFolding.Name = "MenuToggleFolding";
            MenuToggleFolding.ShortcutKeyDisplayString = "Shift+*";
            MenuToggleFolding.Text = "Toggle Folding";
            MenuToggleFolding.Click += new System.EventHandler(MenuToggleFolding_Click);

            // toolStripSeparator9
            toolStripSeparator9.Name = "toolStripSeparator9";
            toolStripSeparator9.Size = new System.Drawing.Size(217, 6);

            // MenuCollapseAll
            MenuCollapseAll.Name = "MenuCollapseAll";
            MenuCollapseAll.ShortcutKeyDisplayString = "Ctrl+Shift+-";
            MenuCollapseAll.Text = "Collapse All";
            MenuCollapseAll.Click += new System.EventHandler(MenuCollapseAll_Click);

            // MenuExpandAll
            MenuExpandAll.Name = "MenuExpandAll";
            MenuExpandAll.ShortcutKeyDisplayString = "Ctrl+Shift++";
            MenuExpandAll.Text = "Expand All";
            MenuExpandAll.Click += new System.EventHandler(MenuExpandAll_Click);

            // toolStripMenuItem2
            toolStripMenuItem2.Name = "toolStripMenuItem2";

            // MenuCut
            MenuCut.Image = Blumind.Properties.Resources.cut;
            MenuCut.Name = "MenuCut";
            MenuCut.ShortcutKeyDisplayString = "Ctrl+X";
            MenuCut.Text = "Cu&t";
            MenuCut.Click += new System.EventHandler(MenuCut_Click);

            // MenuCopy
            MenuCopy.Image = Blumind.Properties.Resources.copy;
            MenuCopy.Name = "MenuCopy";
            MenuCopy.ShortcutKeyDisplayString = "Ctrl+C";
            MenuCopy.Text = "&Copy";
            MenuCopy.Click += new System.EventHandler(MenuCopy_Click);

            // MenuPaste
            MenuPaste.Image = Blumind.Properties.Resources.paste;
            MenuPaste.Name = "MenuPaste";
            MenuPaste.ShortcutKeyDisplayString = "Ctrl+V";
            MenuPaste.Text = "&Paste";
            MenuPaste.Click += new System.EventHandler(MenuPaste_Click);

            // MenuDelete
            MenuDelete.Image = Blumind.Properties.Resources.delete;
            MenuDelete.Name = "MenuDelete";
            MenuDelete.ShortcutKeyDisplayString = "Del";
            MenuDelete.Text = "&Delete";
            MenuDelete.Click += new System.EventHandler(MenuDelete_Click);

            // toolStripSeparator10
            toolStripSeparator10.Name = "toolStripSeparator10";

            // MenuAdvance
            MenuAdvance.Name = "MenuAdvance";
            MenuAdvance.Text = "Advance";
            MenuAdvance.DropDownItems.AddRange(new ToolStripItem[] { MenuNewChartFromHere });

            // MenuNewChartFromHere
            MenuNewChartFromHere.Name = "MenuNewChartFromHere";
            MenuNewChartFromHere.Text = "New Chart From Here";
            MenuNewChartFromHere.Click += MenuNewChartFromHere_Click;

            // MenuEdit
            MenuEdit.Image = Blumind.Properties.Resources.edit;
            MenuEdit.Name = "MenuEdit";
            MenuEdit.ShortcutKeyDisplayString = "F2";
            MenuEdit.Text = "&Edit";
            MenuEdit.Click += new System.EventHandler(MenuEdit_Click);

            // MenuProperty
            MenuProperty.Image = Blumind.Properties.Resources.property;
            MenuProperty.Name = "MenuProperty";
            MenuProperty.Text = "&Property";
            MenuProperty.Click += new System.EventHandler(MenuProperty_Click);

            //
            contextMenu.ResumeLayout();
        }

        protected override void OnChartContextMenuStripOpening(CancelEventArgs e)
        {
            if (SelectedObjects != null && SelectedObjects.Length > 0)
            {
                int count = SelectedObjects.Length;
                Topic topic = mindMapView1.SelectedTopic;
                int topicCount = mindMapView1.SelectedTopics.Length;

                string urls = null;
                MenuOpenHyperlink.Enabled = HasAnyUrl(SelectedObjects, out urls);
                MenuOpenHyperlink.Available = MenuOpenHyperlink.Enabled;
                MenuOpenHyperlink.ToolTipText = urls;

                MenuAddThreat.Visible = !ReadOnly && count == 1 && topicCount > 0 && topic.IsRoot;
                MenuAddConsequence.Visible = !ReadOnly && count == 1 && topicCount > 0 && topic.IsRoot;
                MenuAddBarrier.Visible = !ReadOnly && count == 1 && topicCount > 0 && !topic.IsRoot && (topic.Type == TopicType.Threat || topic.Type == TopicType.Consequence || topic.Type == TopicType.Escalation);
                MenuAddBarrierLeft.Visible = !ReadOnly && count == 1 && topicCount > 0 && topic.Type == TopicType.Barrier;
                MenuAddBarrierRight.Visible = !ReadOnly && count == 1 && topicCount > 0 && topic.Type == TopicType.Barrier;
                MenuAddEscalation.Visible = !ReadOnly && count == 1 && topicCount > 0 && topic.Type == TopicType.Barrier;
                MenuFolding.Available = topicCount > 0 && count == 1 && topic.HasChildren;
                MenuExpandFolding.Enabled = topicCount > 0 && count == 1 && topic.Folded && !topic.IsRoot;
                MenuCollapseFolding.Enabled = topicCount > 0 && count == 1 && !topic.Folded && !topic.IsRoot;
                MenuToggleFolding.Enabled = topicCount > 0 && count == 1 && !topic.IsRoot;
                MenuExpandAll.Enabled = topicCount > 0 && count == 1;
                MenuCollapseAll.Enabled = topicCount > 0 && count == 1;
                MenuAdd.Enabled = true;
                MenuAddIcon.Enabled = topicCount > 0;
                MenuAddProgressBar.Enabled = topicCount > 0;
                MenuAddRemark.Enabled = topicCount > 0;
                MenuNewChartFromHere.Available = topicCount == 1;

                bool hasLink = false;
                foreach (var mo in SelectedObjects)
                {
                    if (mo is Link)
                    {
                        hasLink = true;
                        break;
                    }
                }
                MenuLink.Available = hasLink;
            }
            else
            {
                MenuOpenHyperlink.Enabled = false;
                MenuOpenHyperlink.ToolTipText = null;
                MenuAddThreat.Visible = false;
                MenuAddConsequence.Visible = false;
                MenuAddBarrier.Visible = false;
                MenuAddBarrierLeft.Visible = false;
                MenuAddBarrierRight.Visible = false;
                MenuAddEscalation.Visible = false;
                MenuFolding.Available = false;
                MenuAdd.Enabled = false;
                MenuLink.Available = false;
                MenuNewChartFromHere.Available = false;
            }

            MenuCut.Enabled = mindMapView1.CanCut;
            MenuCopy.Enabled = mindMapView1.CanCopy;
            MenuPaste.Enabled = mindMapView1.CanPaste;
            MenuDelete.Enabled = mindMapView1.CanDelete;
            MenuEdit.Enabled = mindMapView1.CanEdit;
            MenuAdvance.Available = MenuAdvance.HasAvailableItems();

            ChartContextMenuStrip.SmartHideSeparators();
        }

        public override IEnumerable<ExtendActionInfo> GetExtendActions()
        {
            var actions = new List<ExtendActionInfo>();

            if (mindMapView1.CanPasteAsRemark && Clipboard.ContainsText())
            {
                //actions.Add(new ExtendActionInfo("Paste as Note", Properties.Resources.paste_as_remark, MenuPasteAsNote_Click));
            }

            if (mindMapView1.CanPaste && Clipboard.ContainsImage())
            {
                //actions.Add(new ExtendActionInfo("Paste as Image", Properties.Resources.paste_as_image, MenuPasteAsImage_Click));
            }

            Topic topic = mindMapView1.SelectedTopic;
            if (topic != null && !ReadOnly && topic.Type != TopicType.Hazard)
            {
                Topic root = topic.GetRoot();
                if (topic.ParentTopic != null && topic.ParentTopic.Children.Count > 0)
                {
                    int index = topic.Index;
                    XList<Topic> children = topic.ParentTopic.Children;
                    if (topic.ParentTopic.IsRoot)
                    {
                        children = topic.ParentTopic.GetChildrenByType(topic.Type);
                        index = children.IndexOf(topic);
                    }
                    if (index > 0)
                    {
                        if (topic.Type == TopicType.Barrier)
                        {
                            if (topic.Left <= root.Left)
                            {
                                actions.Add(new ExtendActionInfo("Move Right", Properties.Resources.right, KeyMap.MoveRight.ToString(), MenuMoveDown_Click));
                            }
                            else
                            {
                                actions.Add(new ExtendActionInfo("Move Left", Properties.Resources.left, KeyMap.MoveLeft.ToString(), MenuMoveUp_Click));
                            }
                        }
                        else
                            actions.Add(new ExtendActionInfo("Move Up", Properties.Resources.up, KeyMap.MoveUp.ToString(), MenuMoveUp_Click));
                    }

                    if (index < children.Count - 1)
                    {
                        if (topic.Type == TopicType.Barrier)
                        {
                            if (topic.Left <= root.Left)
                            {
                                actions.Add(new ExtendActionInfo("Move Left", Properties.Resources.left, KeyMap.MoveLeft.ToString(), MenuMoveUp_Click));
                            }
                            else
                            {
                                actions.Add(new ExtendActionInfo("Move Right", Properties.Resources.right, KeyMap.MoveRight.ToString(), MenuMoveDown_Click));
                            }
                        }
                        else
                            actions.Add(new ExtendActionInfo("Move Down", Properties.Resources.down, KeyMap.MoveDown.ToString(), MenuMoveDown_Click));
                    }
                }

                if (SelectedObjects.Length == 1 && topic.Children.Count > 1)
                {
                    //actions.Add(new ExtendActionInfo(Lang.GetTextWithEllipsis("Custom Sort"), null, MenuCustomSort_Click));
                }
            }

            return actions;
        }

        bool HasAnyUrl(object[] objects, out string urls)
        {
            urls = null;

            foreach (object obj in objects)
            {
                if (obj is IHyperlink)
                {
                    IHyperlink link = (IHyperlink)obj;
                    if (!string.IsNullOrEmpty(link.Hyperlink))
                    {
                        if (urls == null)
                            urls = link.Hyperlink;
                        else
                            urls += "\n" + link.Hyperlink;
                    }
                }
            }

            return urls != null;
        }

        void MenuOpenHyperlink_Click(object sender, EventArgs e)
        {
            mindMapView1.OpenSelectedUrl();
        }

        void MenuAddThreat_Click(object sender, EventArgs e)
        {
            // mindMapView1.AddTopic();
            mindMapView1.AddThreat();
        }

        void MenuAddConsequence_Click(object sender, EventArgs e)
        {
            mindMapView1.AddConsequence();
        }

        void MenuAddBarrier_Click(object sender, EventArgs e)
        {
            mindMapView1.AddBarrier();
        }

        void MenuAddBarrierLeft_Click(object sender, EventArgs e)
        {
            mindMapView1.AddBarrierLeft();
        }

        void MenuAddBarrierRight_Click(object sender, EventArgs e)
        {
            mindMapView1.AddBarrierRight();
        }

        void MenuAddEscalationFactor_Click(object sender, EventArgs e)
        {
            mindMapView1.AddEscalationFactor();
        }

        void MenuAddIcon_Click(object sender, EventArgs e)
        {
            mindMapView1.AddIcon();
        }

        void MenuAddProgressBar_Click(object sender, EventArgs e)
        {
            mindMapView1.AddProgressBar();
        }

        void MenuAddRemark_Click(object sender, EventArgs e)
        {
            mindMapView1.AddRemark();
        }

        void MenuStraightening_Click(object sender, EventArgs e)
        {
            foreach (ChartObject mo in SelectedObjects)
            {
                if (mo is Link)
                {
                    mindMapView1.ExecuteCommand(new StraighteningCommand((Link)mo));
                }
            }
        }

        void MenuInvert_Click(object sender, EventArgs e)
        {
            foreach (ChartObject mo in SelectedObjects)
            {
                if (mo is Link)
                {
                    mindMapView1.ExecuteCommand(new InvertLinkCommand((Link)mo));
                }
            }
        }

        void MenuCollapseFolding_Click(object sender, EventArgs e)
        {
            Topic topic = mindMapView1.SelectedTopic;
            if (topic != null)
            {
                topic.Collapse();
            }
        }

        void MenuExpandFolding_Click(object sender, EventArgs e)
        {
            Topic topic = mindMapView1.SelectedTopic;
            if (topic != null)
            {
                topic.Expand();
            }
        }

        void MenuToggleFolding_Click(object sender, EventArgs e)
        {
            Topic topic = mindMapView1.SelectedTopic;
            if (topic != null)
            {
                topic.Toggle();
            }
        }

        void MenuCollapseAll_Click(object sender, EventArgs e)
        {
            mindMapView1.CollapseAll();
        }

        void MenuExpandAll_Click(object sender, EventArgs e)
        {
            mindMapView1.ExpandAll();
        }

        void MenuCut_Click(object sender, EventArgs e)
        {
            if (mindMapView1.CanCut)
            {
                mindMapView1.Cut();
            }
        }

        void MenuCopy_Click(object sender, EventArgs e)
        {
            if (mindMapView1.CanCopy)
            {
                mindMapView1.Copy();
            }
        }

        void MenuPaste_Click(object sender, EventArgs e)
        {
            if (mindMapView1.CanPaste)
            {
                mindMapView1.Paste();
            }
        }

        void MenuPasteAsNote_Click(object sender, EventArgs e)
        {
            if (mindMapView1.CanPasteAsRemark)
            {
                mindMapView1.PasteAsRemark();
            }
        }

        void MenuPasteAsImage_Click(object sender, EventArgs e)
        {
            if (mindMapView1.CanPaste)
            {
                mindMapView1.PasteAsImage();
            }
        }

        void MenuDelete_Click(object sender, EventArgs e)
        {
            mindMapView1.DeleteObject();
        }

        void MenuMoveUp_Click(object sender, EventArgs e)
        {
            mindMapView1.CustomSort(-1);
        }

        void MenuMoveDown_Click(object sender, EventArgs e)
        {
            mindMapView1.CustomSort(1);
        }

        void MenuCustomSort_Click(object sender, EventArgs e)
        {
            Topic topic = mindMapView1.SelectedTopic;
            if (topic != null && topic.Children.Count > 1)
            {
                SortTopicDialog dlg = new SortTopicDialog(topic.Children.ToArray());
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    this.mindMapView1.CustomSort(topic, dlg.GetNewIndices());
                }
            }
        }

        void MenuEdit_Click(object sender, EventArgs e)
        {
            mindMapView1.EditObject();
        }

        void MenuProperty_Click(object sender, EventArgs e)
        {
            OnNeedShowProperty(true);
        }

        void MenuNewChartFromHere_Click(object sender, EventArgs e)
        {
            if (mindMapView1.SelectedTopic != null && !ReadOnly && Owner != null)
            {
                var root = mindMapView1.SelectedTopic.Clone();
                root.CutFromMap();
                var map = new MindMap(root, ST.RemoveUnvisibleCharts(root.Text));

                if (ChartThemeManage.Default.DefaultTheme != null)
                {
                    map.ApplyTheme(ChartThemeManage.Default.DefaultTheme);
                }

                root.Expand();
                Owner.Document.Charts.Add(map);
                Owner.ActiveChartPage(map);
            }
        }
        #endregion

        #region Custom Tab Menu
        ToolStripMenuItem MenuLayout { get; set; }

        public override List<ToolStripItem> CustomTabMenuItems()
        {
            if (MindMapChart == null)
                return base.CustomTabMenuItems();

            if (MenuLayout == null)
            {
                MenuLayout = new ToolStripMenuItem(Lang._("Layout Type"));

                //
                foreach (MindMapLayoutType layout in Enum.GetValues(typeof(MindMapLayoutType)))
                {
                    ToolStripMenuItem menuLayout = new ToolStripMenuItem();
                    menuLayout.Text = ST.EnumToString(layout);
                    menuLayout.Tag = layout;
                    menuLayout.Image = MindMapLayoutTypeEditor.GetIcon(layout);
                    menuLayout.Click += new EventHandler(MenuLayout_Click);
                    MenuLayout.DropDownItems.Add(menuLayout);
                }
            }

            foreach (ToolStripMenuItem mi in MenuLayout.DropDownItems)
            {
                mi.Checked = (MindMapLayoutType)mi.Tag == MindMapChart.LayoutType;
                mi.Enabled = !ReadOnly;
            }

            List<ToolStripItem> list = new List<ToolStripItem>();
            list.Add(MenuLayout);
            return list;
        }

        void MenuLayout_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem && MindMapChart != null)
            {
                ToolStripMenuItem mi = (ToolStripMenuItem)sender;
                if (mi.Tag is MindMapLayoutType)
                {
                    MindMapChart.LayoutType = (MindMapLayoutType)mi.Tag;
                }
            }
        }

        #endregion
    }
}
