﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MoneyEntry
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class ExpensesEntities : DbContext
    {
        public ExpensesEntities()
            : base("name=ExpensesEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<tdCategory> tdCategory { get; set; }
        public virtual DbSet<tdType> tdType { get; set; }
        public virtual DbSet<tePerson> tePerson { get; set; }
        public virtual DbSet<teTransaction> teTransaction { get; set; }
        public virtual DbSet<vTrans> vTrans { get; set; }
    
        public virtual int spCategoryLines()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("spCategoryLines");
        }
    
        public virtual int spInsertOrUpdateTransaction(Nullable<int> transactionID, Nullable<decimal> amount, string transactionDesc, Nullable<int> typeId, Nullable<int> categoryId, Nullable<System.DateTime> createdDt, Nullable<int> personID)
        {
            var transactionIDParameter = transactionID.HasValue ?
                new ObjectParameter("TransactionID", transactionID) :
                new ObjectParameter("TransactionID", typeof(int));
    
            var amountParameter = amount.HasValue ?
                new ObjectParameter("Amount", amount) :
                new ObjectParameter("Amount", typeof(decimal));
    
            var transactionDescParameter = transactionDesc != null ?
                new ObjectParameter("TransactionDesc", transactionDesc) :
                new ObjectParameter("TransactionDesc", typeof(string));
    
            var typeIdParameter = typeId.HasValue ?
                new ObjectParameter("TypeId", typeId) :
                new ObjectParameter("TypeId", typeof(int));
    
            var categoryIdParameter = categoryId.HasValue ?
                new ObjectParameter("CategoryId", categoryId) :
                new ObjectParameter("CategoryId", typeof(int));
    
            var createdDtParameter = createdDt.HasValue ?
                new ObjectParameter("CreatedDt", createdDt) :
                new ObjectParameter("CreatedDt", typeof(System.DateTime));
    
            var personIDParameter = personID.HasValue ?
                new ObjectParameter("PersonID", personID) :
                new ObjectParameter("PersonID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("spInsertOrUpdateTransaction", transactionIDParameter, amountParameter, transactionDescParameter, typeIdParameter, categoryIdParameter, createdDtParameter, personIDParameter);
        }
    
        public virtual int spMoneyEntry(Nullable<decimal> amount, string description, string type, string category, Nullable<System.DateTime> created, Nullable<int> personID)
        {
            var amountParameter = amount.HasValue ?
                new ObjectParameter("Amount", amount) :
                new ObjectParameter("Amount", typeof(decimal));
    
            var descriptionParameter = description != null ?
                new ObjectParameter("Description", description) :
                new ObjectParameter("Description", typeof(string));
    
            var typeParameter = type != null ?
                new ObjectParameter("Type", type) :
                new ObjectParameter("Type", typeof(string));
    
            var categoryParameter = category != null ?
                new ObjectParameter("Category", category) :
                new ObjectParameter("Category", typeof(string));
    
            var createdParameter = created.HasValue ?
                new ObjectParameter("Created", created) :
                new ObjectParameter("Created", typeof(System.DateTime));
    
            var personIDParameter = personID.HasValue ?
                new ObjectParameter("PersonID", personID) :
                new ObjectParameter("PersonID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("spMoneyEntry", amountParameter, descriptionParameter, typeParameter, categoryParameter, createdParameter, personIDParameter);
        }
    
        public virtual int spMoneyUpdate(Nullable<int> transactionID, Nullable<decimal> amount, string transactionDesc, string type, string category, Nullable<System.DateTime> createdDate, Nullable<bool> reconciled, string person)
        {
            var transactionIDParameter = transactionID.HasValue ?
                new ObjectParameter("TransactionID", transactionID) :
                new ObjectParameter("TransactionID", typeof(int));
    
            var amountParameter = amount.HasValue ?
                new ObjectParameter("Amount", amount) :
                new ObjectParameter("Amount", typeof(decimal));
    
            var transactionDescParameter = transactionDesc != null ?
                new ObjectParameter("TransactionDesc", transactionDesc) :
                new ObjectParameter("TransactionDesc", typeof(string));
    
            var typeParameter = type != null ?
                new ObjectParameter("Type", type) :
                new ObjectParameter("Type", typeof(string));
    
            var categoryParameter = category != null ?
                new ObjectParameter("Category", category) :
                new ObjectParameter("Category", typeof(string));
    
            var createdDateParameter = createdDate.HasValue ?
                new ObjectParameter("CreatedDate", createdDate) :
                new ObjectParameter("CreatedDate", typeof(System.DateTime));
    
            var reconciledParameter = reconciled.HasValue ?
                new ObjectParameter("Reconciled", reconciled) :
                new ObjectParameter("Reconciled", typeof(bool));
    
            var personParameter = person != null ?
                new ObjectParameter("Person", person) :
                new ObjectParameter("Person", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("spMoneyUpdate", transactionIDParameter, amountParameter, transactionDescParameter, typeParameter, categoryParameter, createdDateParameter, reconciledParameter, personParameter);
        }
    
        public virtual int spUpdateTotals()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("spUpdateTotals");
        }
    
        public virtual ObjectResult<spCategoryUseOverDuration_Result> spCategoryUseOverDuration(Nullable<System.DateTime> start, Nullable<System.DateTime> end, Nullable<int> typeId, Nullable<int> personId, Nullable<decimal> minimum)
        {
            var startParameter = start.HasValue ?
                new ObjectParameter("Start", start) :
                new ObjectParameter("Start", typeof(System.DateTime));
    
            var endParameter = end.HasValue ?
                new ObjectParameter("End", end) :
                new ObjectParameter("End", typeof(System.DateTime));
    
            var typeIdParameter = typeId.HasValue ?
                new ObjectParameter("TypeId", typeId) :
                new ObjectParameter("TypeId", typeof(int));
    
            var personIdParameter = personId.HasValue ?
                new ObjectParameter("PersonId", personId) :
                new ObjectParameter("PersonId", typeof(int));
    
            var minimumParameter = minimum.HasValue ?
                new ObjectParameter("Minimum", minimum) :
                new ObjectParameter("Minimum", typeof(decimal));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spCategoryUseOverDuration_Result>("spCategoryUseOverDuration", startParameter, endParameter, typeIdParameter, personIdParameter, minimumParameter);
        }
    
        public virtual ObjectResult<spTransactionSummationByDuration_Result> spTransactionSummationByDuration(string xml)
        {
            var xmlParameter = xml != null ?
                new ObjectParameter("Xml", xml) :
                new ObjectParameter("Xml", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spTransactionSummationByDuration_Result>("spTransactionSummationByDuration", xmlParameter);
        }
    }
}
