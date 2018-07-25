using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Blumind.Core;
using Blumind.Globalization;

namespace Blumind.Controls.MapViews
{
    partial class MindMapView
    {
        ToolStripButton TsbAddThreat;
        ToolStripButton TsbAddConsequence;
        ToolStripButton TsbAddBarrier;
        ToolStripButton TsbAddEscalation;
        ToolStripButton TsbAddRemark;
        ToolStripButton TsbAddProgressBar;
        ToolStripSeparator toolStripSeparator1;

        bool ToolStripItemsInited;

        public override IEnumerable<ToolStripItem> GetToolStripItems()
        {
            if (!ToolStripItemsInited)
            {
                InitializeToolStripItems();
            }

            return new ToolStripItem[]
            {
                TsbAddThreat,
                TsbAddConsequence,
                TsbAddBarrier,
                TsbAddEscalation,
                //TsbAddIcon,
                //TsbAddRemark,
                //TsbAddProgressBar,
            };
        }

        void InitializeToolStripItems()
        {
            TsbAddThreat = new ToolStripButton();
            TsbAddConsequence = new ToolStripButton();
            TsbAddBarrier = new ToolStripButton();
            TsbAddEscalation = new ToolStripButton();
            TsbAddRemark = new ToolStripButton();
            TsbAddProgressBar = new ToolStripButton();
            toolStripSeparator1 = new ToolStripSeparator();

            // TsbAddThreat
            TsbAddThreat.DisplayStyle = ToolStripItemDisplayStyle.Image;
            TsbAddThreat.Image = Properties.Resources.threat;
            TsbAddThreat.Text = Lang._("Add Threat");
            TsbAddThreat.Click += new System.EventHandler(this.TsbAddThreat_Click);

            // TsbAddConsequence
            TsbAddConsequence.DisplayStyle = ToolStripItemDisplayStyle.Image;
            TsbAddConsequence.Image = Properties.Resources.consequence;
            TsbAddConsequence.Text = Lang._("Add Consequence");
            TsbAddConsequence.Click += new System.EventHandler(this.TsbAddConsequence_Click);

            // TsbAddBarrier
            TsbAddBarrier.DisplayStyle = ToolStripItemDisplayStyle.Image;
            TsbAddBarrier.Image = Properties.Resources.barrier;
            TsbAddBarrier.Text = Lang._("Add Barrier");
            TsbAddBarrier.Click += new System.EventHandler(this.TsbAddBarrier_Click);

            // TsbAddEscalation
            TsbAddEscalation.DisplayStyle = ToolStripItemDisplayStyle.Image;
            TsbAddEscalation.Image = Properties.Resources.escalation;
            TsbAddEscalation.Text = Lang._("Add Escalation");
            TsbAddEscalation.Click += new System.EventHandler(this.TsbAddEscalation_Click);

            // TsbAddRemark
            TsbAddRemark.DisplayStyle = ToolStripItemDisplayStyle.Image;
            TsbAddRemark.Image = Properties.Resources.notes;
            TsbAddRemark.Text = Lang._("Add Remark");
            TsbAddRemark.Click += new System.EventHandler(this.TsbAddRemark_Click);

            // TsbAddProgressBar
            TsbAddProgressBar.DisplayStyle = ToolStripItemDisplayStyle.Image;
            TsbAddProgressBar.Image = Properties.Resources.progress_bar;
            TsbAddProgressBar.Text = Lang._("Add Progress Bar");
            TsbAddProgressBar.Click += new System.EventHandler(this.TsbAddProgressBar_Click);

            ToolStripItemsInited = true;
            ResetControlStatus();
        }

        void TsbAddThreat_Click(object sender, EventArgs e)
        {
            AddThreat();
        }

        void TsbAddConsequence_Click(object sender, EventArgs e)
        {
            AddConsequence();
        }

        void TsbAddBarrier_Click(object sender, EventArgs e)
        {
            //AddLink();
            AddBarrier();
        }

        void TsbAddProgressBar_Click(object sender, EventArgs e)
        {
            AddProgressBar();
        }

        void TsbAddEscalation_Click(object sender, EventArgs e)
        {
            AddEscalationFactor();
        }

        void TsbAddRemark_Click(object sender, EventArgs e)
        {
            AddRemark();
        }

        public override void ResetControlStatus()
        {
            base.ResetControlStatus();

            if (ToolStripItemsInited)
            {
                bool hasSelected = HasSelected() && SelectedTopic != null;

                TsbAddBarrier.Enabled = hasSelected && (SelectedTopic.Type == Model.MindMaps.TopicType.Consequence || SelectedTopic.Type == Model.MindMaps.TopicType.Threat);
                TsbAddConsequence.Enabled = hasSelected && SelectedTopic.IsRoot;
                TsbAddThreat.Enabled = hasSelected && SelectedTopic.IsRoot;
                TsbAddEscalation.Enabled = hasSelected && (SelectedTopic.Type == Model.MindMaps.TopicType.Barrier);
                TsbAddRemark.Enabled = hasSelected;
                TsbAddProgressBar.Enabled = hasSelected;
            }
        }

        protected override void OnCurrentLanguageChanged()
        {
            base.OnCurrentLanguageChanged();

            TsbAddThreat.Text = Lang._("Add Threat", KeyMap.AddThreat.Keys);
            TsbAddConsequence.Text = Lang._("Add Consequence", KeyMap.AddConsequence.Keys);
            TsbAddBarrier.Text = Lang._("Add Barrier", KeyMap.AddBarrier.Keys);
            TsbAddEscalation.Text = Lang._("Add Escalation", KeyMap.AddEscalation.Keys);
            TsbAddProgressBar.Text = Lang._("Add Progress Bar");
            TsbAddRemark.Text = Lang._("Add Remark");
        }
    }
}
