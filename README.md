# ✒️ InkSpace - E-Commerce Platform for Books
[InkSpace Azure Host](https://inkspace-webapp.azurewebsites.net/)

Welcome to **InkSpace**, an e-commerce platform where individuals and businesses can purchase books. Users can create accounts, explore a wide variety of books, and place orders. Companies can register to order books in bulk, and administrators have full control over managing orders and inventory.

## 🌟 Features

### User Features:
- **Account Creation**: Sign up and manage your account profile.
- **Browse and Search**: Find books by category, author, or title using search filters.
- **Order Placement**: Add books to your cart and seamlessly place orders.
- **Order Tracking**: View your order history and track the status of your orders.
- **Admin Actions**: Manage Orders from both users and companies and process payments accordingly.
- **Custom Payment processing**: Custom Payment Methods for companies and retail users.
- **Easy Refunds**: Admins are able to offer seamless refunds with quick processing.
  
### Business Features:
- **Company Accounts**: Businesses can register to place bulk orders.
- **Bulk Orders**: Easily place large orders with discounted pricing.
- **Order Management**: Review and track bulk order statuses.

### Admin Features:
- **Order Management**: Manage orders from both individual users and companies.
- **Inventory Management**: Keep track of book stock and update availability.
- **User Management**: Oversee both personal and company user accounts.

## 🛠️ Technologies Used
- **Backend**: ASP.NET Core
- **Database**: Microsoft SQL Server
- **Payment Processing**: Stripe

## 🚀 Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/your-username/inkspace.git
   cd inkspace

2. Install dependencies:
      ```bash
   dotnet restore

3. Add the appsettings.json file:

   The appsettings.json file is not included in the repository to protect sensitive keys.
    
   Create a new appsettings.json file in the root directory of the project with the following content:

      ```bash
      {
        "Logging": {
          "LogLevel": {
            "Default": "Information",
            "Microsoft.AspNetCore": "Warning"
          }
        },
        "AllowedHosts": "*",
        "ConnectionStrings": {
          "defaultConnection": "Server={The Server For Local Hosted MS Database};Database=InkSpaceDB;User Id=sa;Password=your_password;"
        },
        "Stripe": {
          "SecretKey": "{Your SECRET API KEY FOR STRIPE}",
          "PublishableKey": "{Your Public API KEY FOR STRIPE}"
        }
      }
      ```
   Replace {The Server For Local Hosted MS Database}, {Your SECRET API KEY FOR STRIPE}, and {Your Public API KEY FOR STRIPE} with your actual credentials.

4. Run database migrations to set up the database:

      ```bash
      dotnet ef database update
      ````


5. Run the application:
      ````bash
    dotnet run

## 📋 Usage

### Users:
1. **Sign up** for an account.
2. **Browse** through our collection of books.
3. **Add books** to your cart and checkout using Stripe for payment.
4. **Track orders** in the order history section.

### Companies:
1. **Register** as a company.
2. **Place bulk orders** for books with discounted rates.
3. **Manage** your orders and track delivery progress.

### Administrators:
1. **Log in** with admin credentials.
2. **Manage orders**, inventory, and user accounts from the admin dashboard.

## 📚 Roadmap
- **User Management**: Addition to current admin feature set to manage users both retail and company.
- **Book Reviews**: Enable users to add reviews and ratings for books.
- **Notifications**: Send email notifications for order updates.
