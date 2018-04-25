 


using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using Doe.Ls.EntityBase.Exceptions;
using Doe.Ls.EntityBase.Logging;
using Doe.Ls.EntityBase.Models;
using Doe.Ls.EntityBase.RepositoryBase;
using Doe.Ls.EntityBase.SessionService;
using Doe.Ls.ProjectTemplate.Data;

namespace Doe.Ls.ProjectTemplate.Core.BL.EntityRepositories 
{
    public partial class PositionNoteRepository : BaseRepository<PositionNote> 
    {
        public PositionNoteRepository(IUnitOfWork unitOfWork, ILoggerService loggerService, ISessionService sessionService) : base(unitOfWork, loggerService, sessionService)
        {
        }

        public override IQueryable<PositionNote> List()
        {                       
            return base.List()
                    .Include(ent=>ent.PositionInformation) 
                    .OrderBy(ent=>ent.PositionNoteId);
        }

        public override void Insert(PositionNote entity) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Insert(entity);
        }
        
        public override void Update(PositionNote entity, bool refresh = true) 
        {
            
            if (ValidateEntity(entity).Count > 0) 
            {
                throw new EntityValidationException { Errors = ValidateEntity(entity) };
            }

            base.Update(entity, refresh);
        }

        
       
        

        public IQueryable<PositionNote> FilterPositionNotes(IQueryable<PositionNote> positionNotes, SearchArg searchArg)
        {
            var searchWord = searchArg.Search.ToLower();
            var filteredPositionNote = positionNotes.Where(ent => 
                    ent.PositionNoteId.ToString().Contains(searchWord)
                    || (!string.IsNullOrEmpty(ent.PositionInformation.OlderPositionNumber3) && ent.PositionInformation.OlderPositionNumber3.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.Notes) && ent.Notes.ToLower().Contains(searchWord))
                    || (!string.IsNullOrEmpty(ent.CreatedBy) && ent.CreatedBy.ToLower().Contains(searchWord))
);

            return filteredPositionNote.OrderBy(e => e.PositionNoteId);
        }
    }
}



