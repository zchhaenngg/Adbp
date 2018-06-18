using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Adbp.Paging;

namespace Adbp.Zero.MVC.App_Start
{
    public class DataTableBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(Type modelType)
        {
            if (ModelBinders.Binders.ContainsKey(modelType))
            {
                return null;
            }
            else
            {
                if (modelType == typeof(DataTableColumn))
                {
                    ModelBinders.Binders.Add(modelType, new DataTableColumnModelBinder());
                }
                else if (modelType == typeof(DataTableColumnOrder))
                {
                    ModelBinders.Binders.Add(modelType, new DataTableColumnOrderModelBinder());
                }
                else if (modelType == typeof(DataTableSearch))
                {
                    ModelBinders.Binders.Add(modelType, new DataTableSearchModelBinder());
                }
                return null;
            }
        }

        public static string GetValue(ModelBindingContext context, string modelname = null)
            => context.ValueProvider.GetValue(modelname ?? context.ModelName)?.AttemptedValue;

        public class DataTableColumnModelBinder : IModelBinder
        {
            public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
            {
                return GetValue(bindingContext);
            }
            public DataTableColumn GetValue(ModelBindingContext context)
            {
                var modelname = context.ModelName;

                var model = new DataTableColumn();
                model.Data = DataTableBinderProvider.GetValue(context, $"{ modelname }[data]");
                model.Name = DataTableBinderProvider.GetValue(context, $"{ modelname }[name]");
                if (bool.TryParse(DataTableBinderProvider.GetValue(context, $"{ modelname }[searchable]"), out bool seachable))
                {
                    model.Searchable = seachable;
                }
                if (bool.TryParse(DataTableBinderProvider.GetValue(context, $"{ modelname }[orderable]"), out bool orderable))
                {
                    model.Orderable = orderable;
                }
                model.Search = new DataTableColumnSearch();
                model.Search.Value = DataTableBinderProvider.GetValue(context, $"{ modelname }[search][value]");
                if (bool.TryParse(DataTableBinderProvider.GetValue(context, $"{ modelname }[search][regex]"), out bool regex))
                {
                    model.Search.Regex = regex;
                }
                return model;
            }
        }
        public class DataTableColumnOrderModelBinder : IModelBinder
        {
            public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
            {
                return GetValue(bindingContext);
            }

            public DataTableColumnOrder GetValue(ModelBindingContext context)
            {
                var modelname = context.ModelName;

                var model = new DataTableColumnOrder();
                if (int.TryParse(DataTableBinderProvider.GetValue(context, $"{ modelname }[column]"), out int columnIndex))
                {
                    model.Column = columnIndex;
                }
                model.Dir = DataTableBinderProvider.GetValue(context, $"{ modelname }[dir]");
                return model;
            }
        }
        public class DataTableSearchModelBinder : IModelBinder
        {
            public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
            {
                return GetValue(bindingContext);
            }

            public DataTableSearch GetValue(ModelBindingContext context)
            {
                var modelname = context.ModelName;

                var model = new DataTableSearch();
                model.Value = DataTableBinderProvider.GetValue(context, $"{ modelname }[value]");
                if (bool.TryParse(DataTableBinderProvider.GetValue(context, $"{ modelname }[regex]"), out bool regex))
                {
                    model.Regex = regex;
                }
                return model;
            }
        }
    }
}
