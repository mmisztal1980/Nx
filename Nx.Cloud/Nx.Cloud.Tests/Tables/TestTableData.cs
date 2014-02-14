using Microsoft.WindowsAzure.Storage.Table.DataServices;

namespace Nx.Cloud.Tests.Tables
{
    public class TestTableData : TableServiceEntity
    {
        public string Data1 { get; set; }

        public string Data2 { get; set; }

        public string Data3 { get; set; }

        public override bool Equals(object obj)
        {
            TestTableData other = obj as TestTableData;

            if (other == null)
            {
                return false;
            }

            if (!this.Data1.Equals(other.Data1) ||
                (!this.Data2.Equals(other.Data2)) ||
                (!this.Data3.Equals(other.Data3)))
            {
                return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
