Feature: Create taskboard
	In order to organize my tasks
	As an enterprising person
	I want to create new taskboards

Scenario: Create new taskboard
	Given that I enter "C:\foo\bar.taskboard" in the create taskboard dialog
    When I create a new taskboard
    Then a new taskboard database should be created in "C:\foo\bar.taskboard"
    And a new taskboard should be loaded