//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace vincontrol.WebAPI.DatabaseModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class Question
    {
        public Question()
        {
            this.Answers = new HashSet<Answer>();
        }
    
        public int QuestionId { get; set; }
        public string Title { get; set; }
        public int QuestionTypeId { get; set; }
        public string ShortDesciption { get; set; }
        public Nullable<int> Order { get; set; }
    
        public virtual ICollection<Answer> Answers { get; set; }
        public virtual QuestionType QuestionType { get; set; }
    }
}
