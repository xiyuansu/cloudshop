using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.SqlDal.Members;
using System.Collections.Generic;

namespace Hidistro.SaleSystem.Member
{
	public static class MemberTagHelper
	{
		public static long AddTag(MemberTagInfo tag)
		{
			return new MemberTagDao().Add(tag, null);
		}

		public static bool Update(MemberTagInfo tag)
		{
			return new MemberTagDao().Update(tag, null);
		}

		public static MemberTagInfo GetTagInfo(int tagId)
		{
			return new MemberTagDao().Get<MemberTagInfo>(tagId);
		}

		public static IEnumerable<MemberTagInfo> GetAllTags()
		{
			return new MemberTagDao().Gets<MemberTagInfo>("TagId", SortAction.Asc, null);
		}

		public static void UpdateMemberTags(string userIds, string tagIds)
		{
			string[] tags = tagIds.Split(',');
			string[] users = userIds.Split(',');
			new MemberTagDao().UpdateMemberTags(users, tags);
		}

		public static DbQueryResult GetTags(Pagination query)
		{
			return new MemberTagDao().GetTags(query);
		}

		public static void DeleteTag(int tagId)
		{
			new MemberTagDao().DeleteTag(tagId);
		}

		public static IList<MemberTagInfo> GetTagByMember(string memberTagIds)
		{
			return new MemberTagDao().GetTagByMember(memberTagIds);
		}

		public static int UpdateSingleMemberTags(int userId, string tagIds)
		{
			return new MemberTagDao().UpdateSingleMemberTags(userId, tagIds);
		}

		public static IList<MemberTagInfo> AutoTagsByMember(int userId, int orderCount, decimal orderTotalAmount)
		{
			return new MemberTagDao().AutoTagsByMember(userId, orderCount, orderTotalAmount);
		}
	}
}
