using FinalAssignLibrary.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalAssignLibrary.Classes
{
    /// <summary>
    /// Class object containing the parameters and behaviors for a Group within the
    /// Calf & Cub universe. Groups are created and owned by user Accounts. An Account
    /// may have 0 or 1 owned Groups but may join an unlimited amount of Groups as
    /// a member.
    /// </summary>
    public class Group
    {
        /// <summary>
        /// The primary key for the Group, this ID is set when the group is initially 
        /// added to the Calf & Cub database.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The Account object for the Group owner.
        /// </summary>
        public Account Owner { get; set; }
        /// <summary>
        /// String containing the name of the group. May be set and changed by the
        /// Group Owner.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Bool containing the toggle for whether this group is private or public.
        /// Private groups require invites in order to join where public groups can
        /// be joined by any user. May be set or changed by the Group Owner.
        /// </summary>
        public bool InviteRequired { get; set; }
        /// <summary>
        /// List containing the GroupMembership records that are associated with the
        /// Groups members.
        /// </summary>
        public List<GroupMembership> Members;

        /// <summary>
        /// Constructor which creates a new Group.
        /// </summary>
        /// <param name="owner">Account which owns the group.</param>
        /// <param name="name">String which holds the group name.</param>
        /// <param name="inviteRequired">Boolean which determines whether users 
        /// can freely join this group or if they require an invitation from a 
        /// member with rights.</param>
        public Group(Account owner, string name, bool inviteRequired)
        {
            Owner = owner;
            Name = name;
            InviteRequired = inviteRequired;

            // initializes the list to be populated with the GroupMembership
            // records if required.
            Members = new List<GroupMembership>(); 
        }

        /// <summary>
        /// Constructor used to construct an instance of a group retrieved from the
        /// Group table in the database. Member's list is initialized but values must
        /// be added to it afterwards.
        /// </summary>
        /// <param name="id">Integer containing the Group ID primary key</param>
        /// <param name="owner">Account class containing the user who owns this Group</param>
        /// <param name="name">String containing the name of the Group</param>
        /// <param name="inviteRequired">Boolean determining if Group is public or private</param>
        public Group(int id, Account owner, string name, bool inviteRequired)
        {
            Id = id;
            Owner = owner;
            Name = name;
            InviteRequired = inviteRequired;

            // initializes the list to be populated with the GroupMembership
            // records if required.
            Members = new List<GroupMembership>();
        }

        /// <summary>
        /// Method used to get the total worth of each Member account in a Group and
        /// return a total value to be displayed.
        /// </summary>
        /// <returns>The total worth of all group members combined.</returns>
        public double GetGroupWorth()
        {
            double totalWorth = 0.0;
            // looping through each member of the group...
            foreach (GroupMembership member in Members)
            {
                if (member.Account.Stocks is null)
                {
                    AccountDAL accountDAL = new AccountDAL();
                    member.Account.Stocks = accountDAL.GetStockOwnership(member.Account);
                }
                // adding the member's current total account value...
                totalWorth += member.Account.GetAccountValue();
            }
            return totalWorth;
        }

    }
}
