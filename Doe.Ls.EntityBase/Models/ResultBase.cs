using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using Doe.Ls.EntityBase.Logging;

namespace Doe.Ls.EntityBase.Models
{
    public class ResultBase
    {
        public Status Status { get; set; }

        public string Message { get; set; }

        public string[] Errors
        {
            get
            {
                return DbValidationErrorList.Any() ? DbValidationErrorList.Select(e => e.ErrorMessage).ToArray() : null;
            }
        }

        private List<DbValidationError> _dbValidationErrorList = null;

        protected List<DbValidationError> DbValidationErrorList
        {
            get { return _dbValidationErrorList ?? (_dbValidationErrorList = new List<DbValidationError>()); }
            set { _dbValidationErrorList = value; }
        }

        public void AddError(DbValidationError error)
        {
            if (DbValidationErrorList == null) DbValidationErrorList = new List<DbValidationError>();
            DbValidationErrorList.Add(error);
        }

        public void AddErrors(List<DbValidationError> errors)
        {
            if (DbValidationErrorList == null) DbValidationErrorList = new List<DbValidationError>();
            DbValidationErrorList.AddRange(errors);
        }

        public override string ToString()
        {
            return $"{Status} {Message}";
        }
    }
}
