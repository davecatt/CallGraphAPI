# CallGraphAPI - Used for SC-300 to demonstrate API permissions and scopes and application roles.

Setup:
Create an application registration called CallGraphAPI in an Azure tenant, with multi-tenant authentication and a Mobile and Desktop Applications redirect URL of http://localhost

The CallGraphAPI code is based on https://docs.microsoft.com/en-us/azure/active-directory/develop/tutorial-v2-windows-desktop.
Open this solution in Visual Studio.
View the App.xaml.cs and add in the client ID for the app registration created above.


Demo instructions:

Demo 1 - Add a 3rd Party Application as an Enterprise Application in Azure AD

"CallGraphAPI is a desktop client application running locally. It calls Microsoft Graph and thus it needs to authenticate because Graph is a protected API.
The app is already registered in Azure AD as a multi-tenant app and now has a global application ID and definition."
•	In Azure active directory for the demo tenant, go to Application registrations
•	Select CallGraphAPI
•	Click Authentication in the blade menu
"The application is set to multi-tenant and the redirect used for a .NET Core desktop app is http://localhost.  When the user needs to authenticate, 
a browser will open, redirect to Azure AD and then MSAL will listen on localhost for a response and the token."
•	Open CallGraphAPI solution in Visual Studio
•	View the App.xaml.cs and show how the app is using Microsoft.Identity.Client (MSAL)
"Firstly, let’s just log-in as a user from the app's home tenant.  This is where the app is homed."
•	Run the app
•	Click Call Microsoft Graph API button and sign in as a user from the demo tenant
•	Once the details are shown, click Sign-Out
•	In Azure active directory, go to Enterprise applications and search for CallGraphAPI
•	Show the Application ID in the Overview pane (matches the app registration client ID)
•	Click on the Permissions blade menu and then select the User consent tab
•	Click on the Properties blade menu and Delete to remove the application
"Now let’s sign-in as a user from another tenant.  We do not have an application registration in this tenant."
•	Click Call Microsoft Graph API button and sign in as a user from another Azure tenant
•	Once the details are shown, click Sign-Out
•	In Azure active directory, go to Enterprise applications and search for CallGraphAPI
•	Show the Application ID in the Overview pane (matches the app registration client ID)
•	Click on the Permissions blade menu and then select the User consent tab
•	Click on the Properties blade menu and Delete to remove the application
"From a developer point-of-view, I may only want users to sign-in from tenants who have subscribed to my service.  
I won’t know what that tenant is until they sign in and I get the token.  I need to code logic to decide which tenants are valid and which are not.  
Of course, I might only be dealing with individual users, not tenants so that would be a different logic based on the user’s email.
Let’s sign-in as a user from a tenant which is not a subscriber of my service.  I have simple code in my app to handle this."
•	Click Call Microsoft Graph API button and sign in as a user from an Azure tenant which is excluded in the code (if (authResult.TenantId == "<tenant ID"))
•	Stop the CallGraphAPI application


Demo 2 - Application scopes and permissions

"Now let’s increase the scope that the CallGraphAPI client application is requesting."
•	Open CallGraphAPI solution in Visual Studio
•	In the MainWindow.xaml.cs uncomment the following line of code and comment out the line above it:
string[] scopes = new string[] { "user.read", "People.Read.All" };
•	Run the CallGraphAPI application
•	Click Call Microsoft Graph API button and sign in as an Azure user 
•	Note the consent has increased and one of the consents requires admin approval which this user cannot grant
•	Stop the CallGraphAPI application
•	Re-swap the commented lines

"The client app is now going to call another API as well as Graph and it will need to get the user to consent to those permissions as well.  
The new API is called World Domination.  Why?  Because it’s obviously silly and demonstrates that scopes and permissions are purely app-driven.  
The scope for my Web API is meaningless on its own.  So let’s register this new API first (which will also create an enterprise app for it)
•	In Azure active directory for the demo tenant go to the Application registrations blade menu
•	Click + New registration
•	Name this WorldDominationAPI and set it to single-tenant (but don’t add a redirect URI)
•	Click on the Expose an API blade menu
•	Click + Add a scope
•	If necessary, add the app URI
•	Add a scope for users and admins of World.Domination

•	In the application registration for CallGraphAPI, click the API permissions blade menu
•	Click + Add a permission
•	Select the My APIs tab and click WorldDominationAPI
•	Select the new World.Domination permission
•	Click Add permissions
•	Click the new permission and copy the URI
•	In the MainWindow.xaml.cs add the new permission URI to the following line of code, as in the following:
 string[] scopes = new string[] { "user.read", "api://xxxxx/World.Domination"};
"One benefit of the Microsoft Identity Platform v2 is that it can ask for incremental scopes as needed.  
I’ll sign-in again and the platform should now ask me for the new permission."
•	Run the CallGraphAPI application
•	Click Call Microsoft Graph API button and sign in as an Azure user 
•	Note the consent has increased and is now asking for World.Domination
•	Stop the CallGraphAPI application

"In reality this wouldn’t work because in a token request you can only include scopes from one application at a time.  
To include scopes from a different application, I’d need to make a separate call and get another token.  
There are ways to combine them using API management or if you can access the code for both APIs."


Demo 3 - Appplication Roles

"The CallGraphAPI can read my profile information via Graph and can also take over the world.  
But too many people have been trying to take over the world so I’m going to set up application roles such that users can just read 
profile information and you have to be an evil genius to perform world domination.
I’ll add two app roles.  I’ll assign a user to each of those roles so if that specific user signs in, my client application will know it through the token."
•	In the Application registration for WorldDominationAPI, click the App roles blade menu
•	Create an app role named users which allows users/groups membership
•	Create another app role named evil genius which allows both users/groups and applications membership
"A management app itself could be assigned to this role so I can then use application permissions as well as user delegated permissions."

•	In the Enterprise applications blade menu of the Azure AD, select the WorldDominationAPI
•	Click on the Users and groups blade menu
•	Click + Add user/group
•	Assign a user to the users role
•	Click + Add user/group
•	Assign a different user to the evil genius role
"Now to assign an application to the evil genius role.  I won’t actually do this for the client app because that would mean even the standard user 
gets the higher role through the application permission.  This might apply to an admin or management application."
•	In the Application registration for CallGraphAPI, click the API permissions blade menu
•	Click + Add a permission
•	Select the My APIs tab and select WorldDominationAPI
•	Select the Application Permissions box to see the app role (application)

•	In MainWindows.xaml.cs find and uncomment the following line:

//ResultText.Text =  authResult.AccessToken;

•	Run the CallGraphAPI application
•	Click Call Microsoft Graph API button and sign in as the privilged user 
•	Copy the entire token from the first textbox and paste it into https://jwt.ms
•	Note the Role of evil genius in the decoded token claims
•	Sign out and repeat for the non-privileged

•	Stop the CallGraphAPI application
•	Remove the additional scope from MainWindow.xaml.cs
•	Comment out the ResultText.Text =  authResult.AccessToken;
