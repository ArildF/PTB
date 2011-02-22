Feature: Database version
	In order to know what version of the database I have
	As a user
	I want the database to contain the version

@mytag
Scenario: Get database version from the database
	Then I should be able to read the database version from the database
    And the version should be higher than 0
