using System.ComponentModel;
using System.Data;

namespace PeyDej.Servive
{
    public static class Convertor
    {
        public static Type BaseType(Type oType)
        {
            //#### If the passed oType is valid, .IsValueType and is logicially nullable, .Get(its)UnderlyingType
            if (oType is { IsValueType: true, IsGenericType: true } && oType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return Nullable.GetUnderlyingType(oType);
            }
            return oType;
        }
        public static DataTable ConvertToDataTable<T>(IEnumerable<T> data)
        {
            DataTable table = new();
            foreach (T o in data)
            {
                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(o);
                foreach (PropertyDescriptor prop in properties)
                {
                    if (!table.Columns.Contains(prop.Name))
                    {
                        if (BaseType(prop.PropertyType?.GetType() ?? typeof(object)) == typeof(DateTime))
                        {
                            table.Columns.Add(prop.Name, BaseType(prop.PropertyType) ?? typeof(object));
                        }
                        else
                        {
                            table.Columns.Add(prop.Name, BaseType(prop.PropertyType) ?? typeof(object));
                        }
                    }
                }
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                {
                    row[prop.Name] = prop.GetValue(o) ?? DBNull.Value;
                }
                table.Rows.Add(row);


            }
            return table;
        }
    }
}
