﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MUOffLoad
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class MUPRJEntities : DbContext
    {
        public MUPRJEntities()
            : base("name=MUPRJEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Log> Logs { get; set; }
        public virtual DbSet<EvaluationBatch> EvaluationBatches { get; set; }
        public virtual DbSet<EvaluationBatchItem> EvaluationBatchItems { get; set; }
        public virtual DbSet<OldMarksTable> OldMarksTables { get; set; }
        public virtual DbSet<OldSemesterMark> OldSemesterMarks { get; set; }
        public virtual DbSet<UGFinalResultTable> UGFinalResultTables { get; set; }
        public virtual DbSet<mdbSubjectPaperDetail> mdbSubjectPaperDetails { get; set; }
        public virtual DbSet<MarksCard> MarksCards { get; set; }
        public virtual DbSet<OldResultMap> OldResultMaps { get; set; }
        public virtual DbSet<DegreeMax> DegreeMaxes { get; set; }
        public virtual DbSet<GroupName> GroupNames { get; set; }
        public virtual DbSet<Revaluation> Revaluations { get; set; }
        public virtual DbSet<BookletDetail> BookletDetails { get; set; }
    }
}
