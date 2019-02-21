using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalAssignLibrary.Enumerations;

namespace FinalAssignLibrary.Classes
{    
    /// <summary>
    /// Class containing the parameters and behaviors for a member of a Calf & Cub
    /// group.
    /// </summary>
    public class GroupMembership
    {
        public string InvitedBy;
        public bool InvitePending;
        public Account Account;
        public Group Group;
        public bool HasRights;

        /// <summary>
        /// Constructor which is called when an Account joins a new Group.
        /// </summary>
        /// <param name="user">The Account which initiated the request.</param>
        /// <param name="target">The target Group the user is joining.</param>
        public GroupMembership(Account user, Group target)
        {
            Account = user;
            Group = target;
            HasRights = false;
            InvitePending = false;
            target.Members.Add(this);
        }

        /// <summary>
        /// Constructor which is called when an Account invites a new member to a
        /// Group that they are a member of. Groups flagged with InviteRequired will
        /// require that the member Account has rights to the Group in order to invite
        /// other user Accounts.
        /// </summary>
        /// <param name="target">Group which the invite is for</param>
        /// <param name="user">Account which is targeted by the invite request</param>
        /// <param name="invitedBy">String containing the username for the original person 
        /// who invited this user to the group</param>
        public GroupMembership(Account user, Group target, string invitedBy)
        {
            Group = target;
            Account = user;
            HasRights = false;
            InvitedBy = invitedBy;
            InvitePending = true;
        }

        /// <summary>
        /// Constructor to be used when retrieving data from the database
        /// </summary>
        /// <param name="user">User's Account object</param>
        /// <param name="group">Group which the membership belongs to</param>
        /// <param name="hasRights">Boolean for whether the user has group rights or not</param>
        /// <param name="invitedBy">String containing the username for the original person 
        /// who invited this user to the group</param>
        /// <param name="invitePending">Boolean for whether this is a pending invite or not</param>
        public GroupMembership(Account user, Group group, bool hasRights, 
            string invitedBy, bool invitePending)
        {
            Account = user;
            Group = group;
            HasRights = hasRights;
            InvitedBy = invitedBy;
            InvitePending = invitePending;
        }

        /// <summary>
        /// Method used to determine the member's status within the group and
        /// return the determined Enum value.
        /// </summary>
        /// <returns>GroupMembershipStatus Enum value</returns>
        public GroupMembershipStatus GetMemberStatus()
        {
            if (Account.Username.Equals(Group.Owner.Username))
            {
                return GroupMembershipStatus.Owner;
            }
            else if (HasRights)
            {
                return GroupMembershipStatus.HasRights;
            }
            else
            {
                return GroupMembershipStatus.Member;
            }
        }

        /**********************************************************************  
         *  
         *  THESE METHODS BECAME REDUNDANT AS OUR PROJECT PROGRESSED, THE FOCUS
         *  SWITCHED TO RELY HEAVILY ON USING DATA ACCESS OBJECTS TO PERFORM THESE 
         *  GROUPMEMBERSHIP TASKS.
         *  
         **********************************************************************
        /// <summary>
        /// Method which is used to invite new users to the current Group. This method
        /// validates whether GroupRights are requires to perform the operation or not.
        /// Once the validation occurs, the process is directed to the SendInvites method
        /// for further processing.
        /// </summary>
        /// <param name="target">Account which the invite request is intended for.</param>
        public void InviteUser(Account target)
        {
            if (this.Group.InviteRequired)
            {
                if (HasRights)
                {
                    SendInvite(target);
                }
                else
                {
                    throw new InsufficientGroupPermissionException(this.Account, this, ErrorCodes.InviteUser);
                }
            }
            else
            {
                SendInvite(target);
            }
        }

        /// <summary>
        /// Method which is called by the InviteUser method to perform final validation
        /// and send out the invitation to the targeted user. 
        /// </summary>
        /// <param name="target">Account which is being targeted by the operation.</param>
        public void SendInvite(Account target)
        {
            // Can't sent invites to their own account.
            if (target.Equals(Account))
            {
                throw new InsufficientGroupPermissionException(Account, this, ErrorCodes.UsedOnSelf);
            }
            else
            {
                // Checking if user is already in the group...
                bool alreadyMember = false;
                foreach (GroupMembership g in target.Groups)
                {
                    if (g.Equals(Group))
                    {
                        alreadyMember = true;
                        break;
                    }
                }

                if (!alreadyMember)
                {
                    GroupMembership gm = new GroupMembership(target, this.Group, this.Account.Username);
                    target.GroupInvites.Add(gm);
                }
                else
                {
                    throw new InsufficientGroupPermissionException(Account, this, ErrorCodes.AlreadyMember);
                }

            }
        }

        /// <summary>
        /// Method which validates users permissions and processes the operation to
        /// remove a member from the group.
        /// </summary>
        /// <param name="target">Account which is being targeted by the operation.</param>
        public void KickUser(GroupMembership target)
        {
            if (HasRights)
            {
                // No user can kick their own Account from the Group...
                if (target.Equals(Account))
                {
                    throw new InsufficientGroupPermissionException(Account, this, ErrorCodes.UsedOnSelf);
                }
                // Non-owner user can't kick another user who has rights...
                else if (target.HasRights && (!this.Account.Equals(Group.Owner)))
                {
                    throw new InsufficientGroupPermissionException(Account, this, ErrorCodes.KickUser);
                }
                // User can kick any user that does not have rights...
                // Owner users can kick anyone...
                else if (!(target.HasRights) || this.Account.Equals(Group.Owner))
                {
                    target.Account.LeaveGroup(target);
                }
            }
            else
            {
                // Users require rights to kick other users...
                throw new InsufficientGroupPermissionException(Account, this, ErrorCodes.KickUser);
            }
        }

        /// <summary>
        /// Method which allows the group owner to give rights to the targeted member.
        /// </summary>
        /// <param name="gm">User's GroupMembership which is being granted rights</param>
        public void GiveRights(GroupMembership gm)
        {
            if (Account.Equals(Group.Owner))
            {
                if (!gm.HasRights)
                    gm.HasRights = true;
            }
            else
            {
                throw new InsufficientGroupPermissionException
                    (Account, this, ErrorCodes.GiveRights);
            }
        }

        /// <summary>
        /// Method which allows the group owner to remove rights from the targeted member.
        /// </summary>
        /// <param name="gm">User's GroupMembership which is losing their rights.</param>
        public void RemoveRights(GroupMembership gm)
        {
            if (Account.Equals(Group.Owner))
            {
                if (gm.HasRights)
                    gm.HasRights = false;
            }
            else
            {
                throw new InsufficientGroupPermissionException
                    (Account, this, ErrorCodes.RemoveRights);
            }
        }

        /// <summary>
        /// Method which allows the group owner to change the Group privacy modifier.
        /// This is the modifier which controls whether or not an Invitation is required
        /// in order for users to join the group.
        /// </summary>
        /// <param name="modifier">Boolean modifier. If true, invitations are required in
        /// order to join the group and only members with Rights may send invitations. 
        /// If false, anyone can join and invite to the group.</param>
        public void InviteRequired(bool modifier)
        {
            if (Account.Equals(Group.Owner))
            {
                Group.InviteRequired = modifier;
            }
            else
            {
                throw new InsufficientGroupPermissionException
                    (Account, this, ErrorCodes.ChangeGroupPrivacy);
            }
        }
        */
    }
}
