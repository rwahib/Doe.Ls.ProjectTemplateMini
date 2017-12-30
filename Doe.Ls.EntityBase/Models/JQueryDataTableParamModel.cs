using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web.Mvc;

namespace Doe.Ls.EntityBase.Models {
    /// <summary>
    /// Class that encapsulates most common parameters sent by DataTables plugin
    /// </summary>
    public class JQueryDataTableParamModel {
        /// <summary>
        /// Request sequence number sent by DataTable, same value must be returned in response
        /// </summary>       
        public string sEcho { get; set; }

        /// <summary>
        /// Text used for filtering
        /// </summary>
        public string sSearch { get; set; }
        public string SortColumnName { get; set; }
        public bool SortColumnDesc { get; set; }
        /// <summary>
        /// Another query
        /// </summary>

        public string sSearch2 { get; set; }
        /// <summary>
        /// Another query
        /// </summary>
        public string sSearch3 { get; set; }
        /// <summary>
        /// Another query
        /// </summary>
        public string sSearch4 { get; set; }

        /// <summary>
        /// Number of records that should be shown in table
        /// </summary>
        public int iDisplayLength { get; set; }

        /// <summary>
        /// First record that should be shown(used for paging)
        /// </summary>
        public int iDisplayStart { get; set; }

        /// <summary>
        /// Number of columns in table
        /// </summary>
        public int iColumns { get; set; }

        /// <summary>
        /// Number of columns that are used in sorting
        /// </summary>
        public int iSortingCols { get; set; }

        /// <summary>
        /// Comma separated list of column names
        /// </summary>
        public string sColumns { get; set; }
        
        public string sSortDir_0 { get; set; }
        public int iSortCol_0 { get; set; }

        public static JQueryDataTableParamModel CreateArgument(int start=0,int length=50,string search=null,string sortColumnName=null,bool sortColumnDesc=false )
        {
            return new JQueryDataTableParamModel
            {
                iDisplayStart = start,
                iDisplayLength = length,
                sSearch = search,
                SortColumnName = sortColumnName,
                SortColumnDesc = sortColumnDesc

            };
        }
        public static JQueryDataTableParamModel ParseDataTableParamModel(NameValueCollection queryCollection)
            {
            throw new NotImplementedException();
            }

        }
}