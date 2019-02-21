using FinalAssignLibrary.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalAssignLibrary.Database
{
    /// <summary>
    /// Data Access Layer Class which contains the methods required to retrieving 
    /// and storing data within the Calf & Cub database for tasks which are Group
    /// related.
    /// </summary>
    public class GroupDAL
    {
        /// <summary>
        /// Stores SQL "connected" connection information
        /// </summary>
        ConnectionFactory connection = new ConnectionFactory();
        /// <summary>
        /// SqlDataAdapter used for "offline" connections
        /// </summary>
        SqlDataAdapter groupAdapter = null;
        /// <summary>
        /// DataTable which holds the offline data retrieved from the Adapter
        /// </summary>
        DataTable groupTable = null;

        /// <summary>
        /// Constructor used to facilitate offline Group data. Creates a
        /// DataTable of retrieved Group data.
        /// </summary>
        public GroupDAL()
        {
            string conn = connection.Sql;
            groupAdapter = new SqlDataAdapter("SELECT g.id, g.name, g.private, a.username " +
                                                "FROM [Group] g " +
                                                "LEFT OUTER JOIN account a ON a.ownedGroup = g.id", conn);
            SqlCommandBuilder builder = new SqlCommandBuilder(groupAdapter);
            groupAdapter.UpdateCommand = new SqlCommand("UPDATE [Group] SET name=@name, private=@private, owner=@owner WHERE id=@id");
            groupAdapter.DeleteCommand = new SqlCommand("DELETE FROM [Group] WHERE id=@id");
            groupTable = new DataTable();
            groupAdapter.Fill(groupTable);
        }
        
        /// <summary>
        /// Method used to add a new group into the database. 
        /// </summary>
        /// <param name="group">Group which is being added</param>
        /// <returns>Returns the Group ID for the Group record that was added
        /// to the database</returns>
        public int AddGroup(Group group)
        {
            int id = -1;
            using (SqlConnection cnn = connection.GetConnection())
            using (SqlCommand cmd = new SqlCommand("INSERT INTO [Group] (name, private, owner) " +
                "OUTPUT inserted.ID " +
                "VALUES (@Name, @Private, @Owner)", cnn))
            {
                // gets the StudentID for the record added & returns it
                cnn.Open();
                cmd.Parameters.AddWithValue("@Name", group.Name);
                cmd.Parameters.AddWithValue("@Private", group.InviteRequired);
                cmd.Parameters.AddWithValue("@Owner", group.Owner.Username);
                id = Convert.ToInt32(cmd.ExecuteScalar());                
            }
            return id;
        }

        /// <summary>
        /// Method used to delete a Group from the database (in cases where a Group
        /// has been disbanded by it's owner). Deletes all GroupMembership records 
        /// for the group as well.
        /// </summary>
        /// <param name="group">Group which is being deleted.</param>
        public void DeleteGroup(Group group)
        {
            int deleted = DeleteGroupMembership(group);
            if (deleted > 0)
            {
                DataRow[] rows = groupTable.Select($"id = {group.Id}");
                if (rows.Length > 0)
                {
                    rows[0].Delete();
                    groupAdapter.DeleteCommand.Parameters.AddWithValue("@id", group.Id);
                    groupAdapter.Update(groupTable);
                }
            }
        }

        /// <summary>
        /// Method used to push updates for a Group into the Database. Allows
        /// Group owners to update the Group name or the Group Privacy status or
        /// transfer ownership to another user.
        /// </summary>
        /// <param name="group">Group which is being updated</param>
        public void UpdateGroup(Group group)
        {
            DataRow[] rows = groupTable.Select($"id = {group.Id}");
            if (rows.Length > 0)
            {
                rows[0][1] = group.Name;
                rows[0][2] = group.InviteRequired;
                rows[0][3] = group.Owner;

                groupAdapter.UpdateCommand.Parameters.AddWithValue("@name", group.Name);
                groupAdapter.UpdateCommand.Parameters.AddWithValue("@private", group.InviteRequired);
                groupAdapter.UpdateCommand.Parameters.AddWithValue("@owner", group.Owner.Username);
                groupAdapter.UpdateCommand.Parameters.AddWithValue("@id", group.Id);
                groupAdapter.Update(groupTable);
            }
        }

        /// <summary>
        /// Method used to retrieve a Group from the database by it's Group ID.
        /// </summary>
        /// <param name="id">Integer containing the ID of the Group that is being
        /// requested.</param>
        /// <param name="getMembers">Boolean controlling whether to retrieve the member 
        /// list for the group as well</param>
        /// <returns>Group object which was retrieved from the database.</returns>
        public Group GetGroupById(int id, bool getMembers)
        {
            Group group = null;
            DataRow[] rows = groupTable.Select($"id = {id}");
            if (rows.Length > 0)
            {
                AccountDAL accountDAL = new AccountDAL();
                Account owner = accountDAL.GetAccountByUsername(rows[0].Field<string>(3), false);
                group = owner.OwnerOf;
            }
                        
            List<GroupMembership> members = null;
            if (group != null && getMembers)
            {
                members = GetMembersByGroup(group);
                group.Members = members;
            }
            return group;
        }
        
        /// <summary>
        /// Method used to retrieve all Groups from the database. This method accepts
        /// an Accounts invites and memberships lists so that these Groups can be 
        /// omitted from the final results. (This method is intended to return groups
        /// which the user is not yet assoicated with).
        /// </summary>
        /// <param name="invites">List of invites an Account has pending</param>
        /// <param name="memberships">List of Groups which the Account is a member of</param>
        /// <returns>List of Groups which the user's Account is not yet associated with</returns>
        public List<Group> GetGroups(List<GroupMembership> invites, List<GroupMembership> memberships)
        {
            List<GroupMembership> alreadyMember = null;
            // if both lists are initialized...
            if (!(invites is null) && !(memberships is null))
                // concatenate them into a single list...
                alreadyMember = invites.Concat(memberships).ToList();
            else if (!(memberships is null))
                alreadyMember = memberships;
            else if (!(invites is null))
                alreadyMember = invites;

            List<Group> groups = new List<Group>();
            if(groupTable.Rows.Count > 0)
            {                
                foreach (DataRow row in groupTable.Rows)
                {
                    bool match = false;

                    if(alreadyMember != null)
                        foreach (GroupMembership gm in alreadyMember)
                        {
                            // comparing the group to the list of groups the user is
                            // already associated with. If there is a match, the match bool
                            // is changed to true
                            if (gm.Group.Id == Convert.ToInt32(row["id"]))
                                match = true;
                        }
                    // as long as there is no match, retrieve the group object and add to
                    // the final list.
                    if (!match)
                    {
                        Group group = GetGroupById(Convert.ToInt32(row["id"]), false);
                        groups.Add(group);
                    }
                }
            }
            return groups;
        }

        /// <summary>
        /// Method used to get Group Members. 
        /// </summary>
        /// <param name="group">Group containing the group data whos members list was requested</param>
        /// <returns>Returns a list of GroupMembership records from the Database</returns>
        public List<GroupMembership> GetMembersByGroup(Group group)
        {
            List<GroupMembership> members = new List<GroupMembership>();
            using (SqlConnection cnn = connection.GetConnection())
            using (SqlCommand cmd = new SqlCommand("SELECT * FROM groupmembership " +
                "WHERE groupId=@id AND invitePending=0", cnn))
            {
                cnn.Open();
                cmd.Parameters.AddWithValue("@id", group.Id);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    AccountDAL accountDAL = new AccountDAL();
                    Account user = accountDAL.GetAccountByUsername(reader["username"].ToString(), false);
                    bool hasRights = reader.GetBoolean(2);
                    string invitedBy = reader["invitedby"].ToString();
                    bool invitePending = reader.GetBoolean(4);

                    GroupMembership member = new GroupMembership(user, group, hasRights, invitedBy, invitePending);
                    members.Add(member);
                }
            }
            return members;
        }

        /// <summary>
        /// Method used to retrieve all GroupMemberships by Account. 
        /// </summary>
        /// <param name="username">String containing the username for the user whose
        /// GroupMemberships have been requested.</param>
        /// <returns>Returns a list of GroupMemberships constructed from the database</returns>
        public void GetMembershipsByAccount(Account user)
        {
            List<GroupMembership> groups = null;
            using (SqlConnection cnn = connection.GetConnection())
            using (SqlCommand cmd = new SqlCommand("SELECT * FROM groupmembership " +
                                                    "WHERE username=@un " +
                                                    "AND invitePending=0", cnn))
            {
                cnn.Open();
                cmd.Parameters.AddWithValue("@un", user.Username);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                    groups = new List<GroupMembership>();
                while (reader.Read())
                {
                    Group group = GetGroupById(Convert.ToInt32(reader["groupid"]), false);
                    bool hasRights = reader.GetBoolean(2);
                    string invitedBy = reader["invitedBy"].ToString();
                    bool invitePending = reader.GetBoolean(4);

                    GroupMembership member = new GroupMembership(user, group, hasRights, invitedBy, invitePending);
                    groups.Add(member);
                }
            }
            user.Groups = groups;
        }

        /// <summary>
        /// Method used to get any pending Group invitations from the database
        /// </summary>
        /// <param name="username">String containing the username for the Account
        /// whose pending invites are being requested.</param>
        /// <returns>List of GroupMemberships constructed from the database. Each
        /// GroupMembership will contain TRUE in the InvitePending property.</returns>
        public void GetInvitesByAccount(Account user)
        {
            List<GroupMembership> groups = null;
            using (SqlConnection cnn = connection.GetConnection())
            using (SqlCommand cmd = new SqlCommand("SELECT * FROM groupmembership " +
                                                    "WHERE username=@un " +
                                                    "AND invitePending=1", cnn))
            {
                cnn.Open();
                cmd.Parameters.AddWithValue("@un", user.Username);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                    groups = new List<GroupMembership>();
                while (reader.Read())
                {
                    Group group = GetGroupById(Convert.ToInt32(reader["groupid"]), false);
                    bool hasRights = reader.GetBoolean(2);
                    string invitedBy = reader["invitedBy"].ToString();
                    bool invitePending = reader.GetBoolean(4);

                    GroupMembership member = new GroupMembership(user, group, hasRights, invitedBy, invitePending);
                    groups.Add(member);
                }
            }
            user.GroupInvites = groups;
        }

        /// <summary>
        /// Method used to add a GroupMembership into the Database. Used when a 
        /// member joins or invites another user to a Group.
        /// </summary>
        /// <param name="membership">GroupMembership object to be added into the database.</param>
        public void AddGroupMembership(GroupMembership membership)
        {
            string sql = String.Format("INSERT INTO groupmembership " +
                "VALUES (@username, @groupID, @hasRights, @invitedBy, @invitePending)");

            using (SqlConnection cnn = connection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, cnn))
            {
                cnn.Open();
                cmd.Parameters.AddWithValue("@username", membership.Account.Username);
                cmd.Parameters.AddWithValue("@groupID", membership.Group.Id);
                cmd.Parameters.AddWithValue("@hasRights", membership.HasRights);
                if(membership.InvitedBy == null)
                    cmd.Parameters.AddWithValue("@invitedBy", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@invitedBy", membership.InvitedBy);
                cmd.Parameters.AddWithValue("@invitePending", membership.InvitePending);
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Method used to delete a GroupMembership record from the database.
        /// </summary>
        /// <param name="membership">GroupMembership object to be deleted</param>
        public void DeleteGroupMembership(GroupMembership membership)
        {
            string sql = String.Format("DELETE FROM groupmembership " +
                                        "WHERE username = @username " +
                                        "AND groupID = @groupID");

            using (SqlConnection cnn = connection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, cnn))
            {
                cnn.Open();
                cmd.Parameters.AddWithValue("@username", membership.Account.Username);
                cmd.Parameters.AddWithValue("@groupID", membership.Group.Id);
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Method used to delete all GroupMembership records from the database
        /// for a particular group. Used when processing a Group Deletion request
        /// initiated by a Group owner.
        /// </summary>
        /// <param name="group">Group who's member records are to be deleted</param>
        /// <returns>Integer value for the number of records deleted</returns>
        private int DeleteGroupMembership(Group group)
        {
            int deleted = 0;
            string sql = String.Format("DELETE FROM groupmembership " +
                            "WHERE groupID = @groupID");

            using (SqlConnection cnn = connection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, cnn))
            {
                cnn.Open();
                cmd.Parameters.AddWithValue("@groupID", group.Id);
                deleted = cmd.ExecuteNonQuery();
            }
            return deleted;
        }

        /// <summary>
        /// Method used to push updates for a GroupMembership to the database record.
        /// Used when a user accepts a group invite or has been given group rights.
        /// </summary>
        /// <param name="membership">GroupMembership object to be updated in the database</param>
        public void UpdateGroupMembership(GroupMembership membership)
        {
            string sql = String.Format("UPDATE groupmembership " +
                                        "SET hasRights = @hasRights, " +
                                        "invitePending = @invitePending " +
                                        "WHERE username = @username " +
                                        "AND groupID = @groupID");

            using (SqlConnection cnn = connection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, cnn))
            {
                cnn.Open();
                cmd.Parameters.AddWithValue("@username", membership.Account.Username);
                cmd.Parameters.AddWithValue("@groupID", membership.Group.Id);
                cmd.Parameters.AddWithValue("@hasRights", membership.HasRights);
                cmd.Parameters.AddWithValue("@invitePending", membership.InvitePending);
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Method used to get a GroupMembership object from the database.
        /// </summary>
        /// <param name="username">String containing the username of the Account which
        /// this GroupMembership is associated with.</param>
        /// <param name="groupId">Integer containing the GroupID which this GroupMembership
        /// is associated with.</param>
        /// <returns>Returns a matching GroupMembership object constructed from 
        /// the database.</returns>
        public GroupMembership GetGroupMembership(string username, int groupId)
        {
            GroupMembership membership = null;
            using (SqlConnection cnn = connection.GetConnection())
            using (SqlCommand cmd = new SqlCommand("SELECT * FROM groupmembership " +
                                                    "WHERE username=@un " +
                                                    "AND groupId=@groupId", cnn))
            {
                cnn.Open();
                cmd.Parameters.AddWithValue("@un", username);
                cmd.Parameters.AddWithValue("@groupId", groupId);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    AccountDAL accountDAL = new AccountDAL();
                    Account user = accountDAL.GetAccountByUsername(username, false);
                    Group group = GetGroupById(Convert.ToInt32(groupId), false);
                    bool hasRights = reader.GetBoolean(2);
                    string invitedBy = reader["invitedBy"].ToString();
                    bool invitePending = reader.GetBoolean(4);

                    membership = new GroupMembership(user, group, hasRights, invitedBy, invitePending);
                }
            }
            return membership;
        }

    }
}
