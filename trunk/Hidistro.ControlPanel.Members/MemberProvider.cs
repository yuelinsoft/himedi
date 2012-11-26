using Hidistro.Core.Entities;
using Hidistro.Entities.Members;
using System;
using System.Collections.Generic;
using System.Data;
using Hidistro.Core;

namespace Hidistro.ControlPanel.Members
{
    public abstract class MemberProvider
    {
        static readonly MemberProvider _defaultInstance = (DataProviders.CreateInstance("Hidistro.ControlPanel.Data.MemberData,Hidistro.ControlPanel.Data") as MemberProvider);

        protected MemberProvider()
        {
        }

        public abstract bool CreateMemberGrade(MemberGradeInfo memberGrade);
        public abstract bool DealBalanceDrawRequest(int userId, bool agree);
        public abstract bool Delete(int userId);
        public abstract bool DeleteMemberGrade(int gradeId);
        public abstract DbQueryResult GetBalanceDetails(BalanceDetailQuery query);
        public abstract DbQueryResult GetBalanceDetailsNoPage(BalanceDetailQuery query);
        public abstract DbQueryResult GetBalanceDrawRequests(BalanceDrawRequestQuery query);
        public abstract DbQueryResult GetBalanceDrawRequestsNoPage(BalanceDrawRequestQuery query);
        public abstract DbQueryResult GetMemberBlanceList(MemberQuery query);
        public abstract MemberGradeInfo GetMemberGrade(int gradeId);
        public abstract IList<MemberGradeInfo> GetMemberGrades();
        public abstract DbQueryResult GetMembers(MemberQuery query);
        public abstract DataTable GetMembersNopage(MemberQuery query, IList<string> fields);
        public abstract bool HasSamePointMemberGrade(MemberGradeInfo memberGrade);
        public abstract bool InsertBalanceDetail(BalanceDetailInfo balanceDetail);
        public static MemberProvider Instance()
        {
            return _defaultInstance;
        }

        public abstract void SetDefalutMemberGrade(int gradeId);
        public abstract bool UpdateMemberGrade(MemberGradeInfo memberGrade);
    }
}

