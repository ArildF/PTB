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
    And the new task should be in edit mode
	And the new task should be selected
    And the task should not be started


Scenario: Created date
	Given an open taskboard
    When I click new task
    Then the new task should have a created date like now
    And the new task should have a modified date like now

Scenario: Subtasks
	Given that the following tasks already exist and are loaded:
    |Title      |
    |Yo         |
	When I select task 'Yo'
	And click new subtask
	Then a new task should be created
	And the new task should be in edit mode
	And the new task should be in position #2
	And the new task should be indented 1 place
	And the new task should be selected
