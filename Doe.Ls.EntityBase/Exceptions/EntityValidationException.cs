using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;

namespace Doe.Ls.EntityBase.Exceptions {
    public class EntityValidationException : Exception {
        public List<DbValidationError> Errors { get; set; }
    }
}
