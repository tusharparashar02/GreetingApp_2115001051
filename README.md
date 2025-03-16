## GreetingApp
### Use Case = 1 
- Using GreetingController return JSON for different HTTP Methods. 
### Use Case = 2
- Extend GreetingController to use Services Layer to get Simple Greeting message ”Hello World”
### Use Case = 3 
- Ability for the Greeting App to give Greeting message with 
- 1. User First Name and Last Name or 
- 2. With just First Name or Last Name based on User attributes provides or 
- 3. Just Hello World. 
### Use Case = 4 
- Ability for the Greeting App to save the Greeting Message in the Repository 
### Use Case = 5 
- Ability for the Greeting App to find a Greeting Message by Id in the Repository 
### Use Case = 6 
- Ability for the Greeting App to List all the Greeting Messages in the Repository 
### Use Case = 7 
- Ability for the Greeting App to Edit a Greeting Messages in the Repository 
### Use Case = 8 
- Ability for the Greeting App to delete a Greeting Messages in the Repository and apply NLog
### Use Case = 9 
- Apply swagger and global exception in the project. 
### Use Case = 10 
-Previous controller will have all the api's from UC1 to UC8 Create Second controller(UserController) where it will contain api for login, registration, forget password and reset password. Apply logic for register and login api for user. Apply hashing algo(salting) for register and login. 
### Use Case = 11 
- Apply JWT into project. 
### Use Case = 12 
- Give body to Forget password by applying SMTP server mail generation and sending token generated using jwt in the mail. Give body to Reset password using JWT token received in mail.
### Use Case = 13 
- Create Primary/Foreign key relationship between greeting and user table. Adjust greeting methods according to new relationship. 
### Use Case = 14 
- Apply Redis and RabbitMQ
