Feature: Creating new tasks
	In order to remember stuff that needs to be done
	As a person
	I want to be able to add new tasks

Scenario: Add new tasks
	Given that the following tasks already exist and are loaded:
    |Title      |
    |Yo         |
    When I click new task
    Then a new task should be created
    And the new task should be displayed first

