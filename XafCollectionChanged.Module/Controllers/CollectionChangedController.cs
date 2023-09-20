using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace XafCollectionChanged.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class CollectionChangedController : ViewController
    {
        // Use CodeRush to create Controllers and Actions with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/403133/
        public CollectionChangedController()
        {
            InitializeComponent();
            this.TargetViewType = ViewType.DetailView;
            this.TargetViewNesting = Nesting.Root;
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        void Collection_CollectionChanged(object sender, XPCollectionChangedEventArgs e)
        {
            Debug.WriteLine("CollectionChanged:"+e.CollectionChangedType.ToString());
        }
        void View_CurrentObjectChanged(object sender, EventArgs e)
        {
            MapEvents();
        }

        private void MapEvents()
        {
            if (this.View.CurrentObject != null)
            {
                var Instance = this.View.CurrentObject as BaseObject;
                Instance.ClassInfo.Members.Where(m => m.IsCollection).ToList().ForEach(m =>
                {
                    var collection = Instance.GetMemberValue(m.Name) as XPBaseCollection;
                    collection.CollectionChanged += Collection_CollectionChanged;
                });
            }
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            this.View.CurrentObjectChanged += View_CurrentObjectChanged;
            MapEvents();
            // Perform various tasks depending on the target View.
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
    }
}
