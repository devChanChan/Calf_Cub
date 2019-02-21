using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalAssignLibrary.Enumerations;
using FinalAssignLibrary.Database;

namespace FinalAssignLibrary.Classes
{
    /// <summary>
    /// Class which contains all the data and behaviors relating to a user's Account
    /// in Calf & Cub.
    /// </summary>
    public class Account
    {
        /// <summary>
        /// Constant value for the starting balance each player receives when 
        /// registering an account or a new season starts.
        /// </summary>
        public const double STARTING_BALANCE = 10000.00; 
        /// <summary>
        /// The primary key for Account is the string containing the username.
        /// </summary>
        private string username;
        public string Username
        {
            get { return username; }
            private set { username = value; }
        }
        /// <summary>
        /// Email does not really have a use in our application at this time but is
        /// collected from User Registrations and stored in the database.
        /// </summary>
        private string email;
        public string Email
        {
            get { return email; }
            set { email = value; }
        }
        /// <summary>
        /// The hashed password string
        /// </summary>
        private string password;
        public string Password
        {
            get { return password; }
            set { password = value; }
        }
        /// <summary>
        /// The salt value used when hashing the user's password
        /// </summary>
        private string salt;
        public string Salt
        {
            get { return salt; }
            set { salt = value; }
        }
        /// <summary>
        /// The current account balance which the user uses to buy stocks
        /// </summary>
        private double balance;
        public double Balance
        {
            get { return balance; }
            set { balance = value; }
        }
        /// <summary>
        /// A Group object pointing to the group which the user is the owner of.
        /// There is a limit of 1 owned Group per user.
        /// </summary>
        private Group ownerOf = null;
        public Group OwnerOf
        {
            get { return ownerOf; }
            set { ownerOf = value; }
        }
        /// <summary>
        /// List of groups which this user is a member of
        /// </summary>
        public List<GroupMembership> Groups = null;
        /// <summary>
        /// List of groups which this user has pending invitations for
        /// </summary>
        public List<GroupMembership> GroupInvites = null;
        /// <summary>
        /// List of stocks which this user has purchased or sold
        /// </summary>
        public List<StockOwnership> Stocks = null;

        /// <summary>
        /// Constructor to allow for registration of a new user Account.
        /// </summary>
        /// <param name="username">A string containing the username for login</param>
        /// <param name="email">A string containing the users email address for communications</param>
        /// <param name="password">A string which contains the users password</param>
        /// <param name="salt">A string which conatins the salt value used to encrypt users password</param>
        public Account(string username, string email, string password, string salt)
        {
            this.Username = username;
            this.Email = email;
            this.Password = password;
            this.Salt = salt;
            this.Balance = STARTING_BALANCE; // initializes the starting balance
        }

        /// <summary>
        /// Constructor used for basic creation of account objects on log in request.
        /// Retrieves minimal data from the database to process the request.
        /// </summary>
        /// <param name="username">A string containing the username for login</param>
        /// <param name="password">A string which contains the users password</param>
        /// <param name="salt">A string which conatins the salt value used to encrypt users password</param>
        public Account(string username, string password, string salt)
        {
            this.Username = username;
            this.Password = password;
            this.Salt = salt;
        }

        /// <summary>
        /// Constructor used for basic creation of account objects. Retrieves minimal
        /// data from the database to construct the Account object. The Account lists
        /// may be populated as required by the UI Controller creating the instance of
        /// the Account. 
        /// </summary>
        /// <param name="username">String containing the Account's username</param>
        /// <param name="balance">Double containing the current Account balance</param>
        /// <param name="groupId">Int containing the group ID for the group which this user owns</param>
        /// <param name="name">String containing the name for the group which this user owns</param>
        /// <param name="isPrivate">Bool containing the privacy status for the group which this user owns</param>
        public Account(string username, double balance, int groupId, string name, bool isPrivate)
        {
            this.Username = username;
            this.Balance = balance;

            // constructs the user's owned Group object (if they own one)
            if(groupId > 0)
                this.OwnerOf = new Group(groupId, this, name, isPrivate);


            // initializing empty lists in preparation to receive data if required.
            this.Stocks = new List<StockOwnership>();
            this.GroupInvites = new List<GroupMembership>();
            this.Groups = new List<GroupMembership>();
        }

        /// <summary>
        /// Method used when the value of the current Account is requested.
        /// </summary>
        /// <returns>Returns a double value containing the total value of the 
        /// current Account calculated by getting the value of every StockOwnership
        /// which the current Account posesses and adding the Account Balance value.
        /// </returns>
        public double GetAccountValue()
        {
            double acctValue = 0.0;
            if (this.Stocks != null)
            {
                if (this.Stocks.Count > 0)
                {
                    // retrieving the current value of each StockOwnership record the user has
                    foreach (StockOwnership so in Stocks)
                    {
                        acctValue += (so.Stock.ValuePerShare * so.GetTotalQuantity());
                    }
                }
            }
            acctValue += Balance; // adding the users current account balance

            return acctValue;
        }

        /// <summary>
        /// Method used to calculate and return the current profit rate for the Account
        /// so that the user can see at a glance how they are performing.
        /// </summary>
        /// <returns>Float value containing the profit rate percentage.</returns>
        public double GetAccountPerformance()
        {
            double currProfitRate = 0.0; // profit rate for the account

            // caculate the profit rate of the Account based on the starting balance $10,000
            currProfitRate = (this.GetAccountValue() - 10000) / 10000;

            return currProfitRate;
        }


        /**********************************************************************  
         *  
         *  THESE METHODS BECAME REDUNDANT AS OUR PROJECT PROGRESSED, THE FOCUS
         *  SWITCHED TO RELY HEAVILY ON USING DATA ACCESS OBJECTS TO PERFORM THESE 
         *  ACCOUNT TASKS.
         *  
         **********************************************************************
        /// <summary>
        /// Method which allows for an Account to join a Group via constructing of a
        /// GroupMembership object and adding the constructed object to their groups
        /// listing. Only public groups may be joined by Accounts.
        /// </summary>
        /// <param name="group">The target group which the Account is attempting to join.</param>
        public void JoinGroup(Group group)
        {
            if (group.InviteRequired)
            {
                throw new GroupIsPrivateException(group);
            }
            else
            {
                GroupMembership gm = new GroupMembership(this, group);

                if (this.Groups == null)
                    InitializeGroups();

                Groups.Add(gm);
            }
        }

        /// <summary>
        /// Method which allows for an Account to join a group via accepting a GroupInvite
        /// request. The Group will be removed from their GroupInvite list and added to
        /// their Groups list. The Group.Members list will also be updated to add the user's
        /// GroupMembership. GroupMembership.InvitePending is also updated to false.
        /// </summary>
        /// <param name="gm">GroupMembership object from the GroupInvites list which the 
        /// user is accepting</param>
        public void JoinGroup(GroupMembership gm)
        {
            if (this.Groups == null)
                InitializeGroups();

            Groups.Add(gm);
            gm.Group.Members.Add(gm);
            gm.InvitePending = false;
        }

        /// <summary>
        /// Method which allows for an Account to join a Group. This method is used
        /// only when an Account is creating a new Group to add the group owner into
        /// the group and bypass the private group modifier. It also automatically 
        /// gives the owner Account rights to their own group.
        /// </summary>
        /// <param name="group">The target group which the Account created and will join</param>
        /// <param name="bypass">The boolean modifier to control access to this method</param>
        public void JoinGroup(Group group, bool bypass)
        {
            if(bypass)
            {
                GroupMembership gm = new GroupMembership(this, group);
                gm.HasRights = true;

                if (this.Groups == null)
                    InitializeGroups();

                Groups.Add(gm);
            }
        }

        /// <summary>
        /// Method used to initialize the list which holds GroupMembership objects
        /// for this user account.
        /// </summary>
        public void InitializeGroups()
        {
            this.Groups = new List<GroupMembership>();
        }

        /// <summary>
        /// Method called when an Account initiates a request to leave a Group which
        /// they are a member of. The method removes the GroupMembership object from
        /// the Account Groups listing and the Group Members listing before being
        /// destroyed.
        /// </summary>
        /// <param name="gm">The GroupMembership to be destroyed.</param>
        public void LeaveGroup(GroupMembership gm)
        {
            if (gm.Account.Groups.Count == 1)
                gm.Account.Groups = null;
            else
                gm.Account.Groups.Remove(gm);

            gm.Group.Members.Remove(gm);
            gm = null;
        }
    

        /// <summary>
        /// Method called when a user initiates the ResetAccount process on their own
        /// Account. Method will destroy all existing StockOwnership objects associated
        /// with this Account and set the Account.balance to the STARTING_BALANCE amount
        /// and reset the Account.avgCurrRate to 0.0.
        /// </summary>
        public void ResetAccount()
        {
            
            foreach(StockOwnership so in Stocks) {
                so.Transactions = null;
            }
            Stocks = null;
            
            balance = STARTING_BALANCE;
        }

        /// <summary>
        /// Method used when the user Account accepts a GroupMembership from their
        /// GroupInvites list. The GroupMembership object is removed from the GroupInvites
        /// list and then is passed to the JoinGroup method for processing.
        /// </summary>
        /// <param name="gm">GroupMembership which the user is accepting.</param>
        public void AcceptGroupInvite(GroupMembership gm)
        {
            GroupInvites.Remove(gm);
            JoinGroup(gm);
        }

        /// <summary>
        /// Method used when the user Account rejects a GroupMembership from their
        /// GroupInvites list. The GroupMembership object is removed from the GroupInvites
        /// list and then is destroyed.
        /// </summary>
        /// <param name="gm">GroupMembership which the user is rejecting.</param>
        public void RejectGroupInvite(GroupMembership gm)
        {
            GroupInvites.Remove(gm);
            gm = null;
        }

        /// <summary>
        /// Method used when adding a new Stock association to the user Account
        /// Will add the StockOwnership to the stocks list and also attempt to 
        /// create a new Transaction inside the new StockOwnership to process the
        /// users Buy request.
        /// </summary>
        /// <param name="stock">Stock which the user is attempting to buy.</param>
        /// <param name="qty">Integer containing the amount of shares the user 
        /// is attempting to buy.</param>
        public void AddStock(Stock stock, int qty)
        {
            StockOwnership stockRecord = null;

            if (Stocks == null)
            {
                Stocks = new List<StockOwnership>();
            } 
            else
            {
                foreach (StockOwnership so in Stocks)
                {
                    if (so.Stock.Equals(stock))
                    {
                        stockRecord = so;
                        break;
                    }
                }
            }

            if(stockRecord == null)
            {
                stockRecord = new StockOwnership(this, stock);
            }

            Stocks.Add(stockRecord);

            if ((stock.ValuePerShare * qty) <= Balance)
            {
                Transaction t = stockRecord.Buy(qty);
                Balance -= t.TransactionAmount;
            }
            else
            {
                throw new BalanceInsufficientException(stock, this, qty);
            }

        }

        /// <summary>
        /// Method which is called when Account attempts to sell off shares contained
        /// in their StockOwnership portfolio.
        /// </summary>
        /// <param name="so">StockOwnership which is being targeted</param>
        /// <param name="qty">Integer containing the amount of shares the user is 
        /// attempting to sell.</param>
        public void RemoveStock(StockOwnership so, int qty)
        {
            so.Sell(qty);
        }





        /*
         * Nov 16, 2018: 
         * REMOVING FRIEND FUNCTIONALITY FOR NOW DUE TO PROJECT DEADLINE CONSTRAINTS
         * 
         * 
        private List<Account> Friends; 
        public List<Account> FriendRequests; 

        /// <summary>
        /// Method called when an Account attempts to add another Account to their
        /// Friends list. Initiates a Friend request to the targeted Account.
        /// </summary>
        /// <param name="target">Account which the user is trying to add to their Friendslist.</param>
        public void AddFriend(Account target)
        {
            if (target.FriendRequests == null)
                target.FriendRequests = new List<Account>();

            target.FriendRequests.Add(this);
        }

        /// <summary>
        /// Method called when an Account initiates the Remove Friend process. Removes
        /// the current Account from the target Account's Friends list and then removes
        /// the target Account from the current Account's Friends list.
        /// </summary>
        /// <param name="target">Account which user is removing from their Friends List.</param>
        public void RemoveFriend(Account target)
        {
            if (Friends.Count == 1)
                Friends = null;
            else
                Friends.Remove(target);

            if (target.Friends.Count == 1)
                target.Friends = null;
            else
                target.Friends.Remove(this);
        }

        /// <summary>
        /// Method called when an Account accepts a friend request from their 
        /// FriendRequests list.
        /// </summary>
        /// <param name="target">Account object to be removed from the current Account's
        /// FriendRequests list and added to their Friends list.</param>
        public void AcceptFriend(Account target)
        {
            if (FriendRequests.Count == 1)
                FriendRequests = null;
            else
                FriendRequests.Remove(target);

            if (Friends == null)
                Friends = new List<Account>();

            this.Friends.Add(target);
        }

        /// <summary>
        /// Method called when an Account rejects a friend request from their 
        /// FriendRequests list
        /// </summary>
        /// <param name="target">Account to be removed from the current Account's 
        /// FriendRequests list.</param>
        public void RejectFriend(Account target)
        {
            if (FriendRequests.Count == 1)
                FriendRequests = null;
            else 
                FriendRequests.Remove(target);
        }
        */
    }
}
