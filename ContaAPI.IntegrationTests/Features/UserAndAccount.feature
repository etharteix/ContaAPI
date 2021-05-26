Feature: UserAndAccount

@userAndAccountOperations
Scenario: Test all the operations
	Given the user's data:
	| Name           | Email               | Password |
	| User Test      | emailtest@test.test | asd123   |
	When the register is done
	And i recover the user by id
	Then the user exists, returned, on the database
	Given the login data:
	| Email               | Password |
  | emailtest@test.test | asd123   |
	When the login is done
	Given the user's update data:
	| Name              | Email                | Password     |
	| User Test 2       | emailtest2@test.test | asd1234      |
	When the update is done
	And i recover the updated user by id
	Given the updated login data:
	| Email                | Password  |
  | emailtest2@test.test | asd1234   |
	When the updated login is done
	Given the deposit data:
	| Value   |
  | 2000000 |
	When the deposit is done
	And i recover the account by userId
	Then the deposit resulting value is returned on the database
	Given the withdraw data:
	| Value   |
  | 500000  |
	When the withdraw is done
	And i recover the account by userId
	Then the withdraw resulting value is returned on the database
	Given the payment data:
	| Value   | PixCode       |
  | 500000  | pix@test.test |
	When the payment is done
	And i recover the account by userId
	Then the payment resulting value is returned on the database
	When the monetize is done
	And i recover the account by userId
	Then the monetize resulting value is returned on the database
