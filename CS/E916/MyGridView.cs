using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Registrator;
using DevExpress.XtraGrid.Views.Base;
using System.ComponentModel;
using DevExpress.XtraGrid.Columns;
using DevExpress.Data;

namespace DXSample {
    public class MyGridControl : GridControl {
        public MyGridControl() : base() { }

        protected override void RegisterAvailableViewsCore(InfoCollection collection) {
            base.RegisterAvailableViewsCore(collection);
            collection.Add(new MyGridViewInfoRegistrator());
        }
    }

    public class MyGridView : GridView {
        public MyGridView() : base() { }
        public MyGridView(GridControl grid) : base(grid) { }

        internal const string MyGridViewName = "MyGridView";
        protected override string ViewName { get { return MyGridViewName; } }

        private string fsFieldName;
        public string SortFieldName {
            get { return fsFieldName; }
            set { fsFieldName = value; }
        }

        protected override void OnColumnSortInfoCollectionChanged(CollectionChangeEventArgs e) {
            base.OnColumnSortInfoCollectionChanged(e);
            GridColumn col = Columns[fsFieldName];
            if (col == null) return;
            if (col.SortIndex != 0) {
                SortInfo.BeginUpdate();
                try {
                    if (Columns[fsFieldName].SortIndex > 0)
                        SortInfo.RemoveAt(Columns[fsFieldName].SortIndex);
                    SortInfo.Insert(0, new GridColumnSortInfo(Columns[fsFieldName], ColumnSortOrder.Ascending));
                } finally { SortInfo.EndUpdate(); }
            }

        }

        public override void Assign(BaseView v, bool copyEvents) {
            BeginUpdate();
            try {
                base.Assign(v, copyEvents);
                MyGridView source = v as MyGridView;
                if (source == null) return;
                SortFieldName = source.SortFieldName;
            } finally { EndUpdate(); }
        }
    }

    public class MyGridViewInfoRegistrator : GridInfoRegistrator {
        public MyGridViewInfoRegistrator() : base() { }

        public override string ViewName { get { return MyGridView.MyGridViewName; } }

        public override BaseView CreateView(GridControl grid) {
            return new MyGridView(grid);
        }
    }
}