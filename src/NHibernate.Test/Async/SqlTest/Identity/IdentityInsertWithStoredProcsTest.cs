﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using NHibernate.Cfg;
using NUnit.Framework;

namespace NHibernate.Test.SqlTest.Identity
{
	using System.Threading.Tasks;
	[TestFixture]
	public abstract class IdentityInsertWithStoredProcsTestAsync : TestCase
	{
		protected override string MappingsAssembly
		{
			get { return "NHibernate.Test"; }
		}

		protected override void Configure(NHibernate.Cfg.Configuration configuration)
		{
			configuration.SetProperty(Environment.FormatSql, "false");
		}

		protected abstract string GetExpectedInsertOrgLogStatement(string orgName);

		/// <summary>
		/// Organization should be mappend with "identity" id strategy AND custom sql-insert (a stored proc).
		/// The insert stored proc will return the new primary key and NH should recognize it and apply it
		/// just like a normal insert.
		/// </summary>
		[Test]
		public async Task InsertUsesStoredProcAsync()
		{
			using (var spy = new SqlLogSpy())
			{
				Organization ifa;
				using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					ifa = new Organization("IFA");
					await (s.SaveAsync(ifa));
					await (t.CommitAsync());
				}

				Assert.AreEqual(1, spy.Appender.GetEvents().Length, "Num loggedEvents");
				Assert.AreEqual(1, ifa.Id, "ifa.Id");
				Assert.AreEqual(GetExpectedInsertOrgLogStatement("IFA"), spy.Appender.GetEvents()[0].RenderedMessage, "Message 1");
				using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					await (s.DeleteAsync(ifa));
					await (t.CommitAsync());
				}
			}

			using (var spy = new SqlLogSpy())
			{
				Organization efa;
				using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					efa = new Organization("EFA");
					await (s.SaveAsync(efa));
					await (t.CommitAsync());
				}

				Assert.AreEqual(1, spy.Appender.GetEvents().Length, "Num loggedEvents");
				Assert.AreEqual(2, efa.Id, "efa.Id");
				Assert.AreEqual(GetExpectedInsertOrgLogStatement("EFA"), spy.Appender.GetEvents()[0].RenderedMessage, "Message 2");
				using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					await (s.DeleteAsync(efa));
					await (t.CommitAsync());
				}
			}
		}
	}
}
