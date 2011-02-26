Feature: Change task state
	In order to properly reflect the actual state of tasks
	As a user
	I want to change the state of tasks

Background: 
    Given that the following tasks already exist and are loaded:
    |Title      |
    |Do stuff         |

Scenario: Set task started
	When I drag task #1 to the "In Progress" column
	Then task #1 should be "In Progress"


Scenario: Set task completed
	When I drag task #1 to the "Complete" column
	Then task #1 should be "Complete"

Scenario: Set task abandoned
	When I drag task #1 to the "Abandoned" column
	Then task #1 should be "Abandoned"

Scenario: Drag back to not started
    When I drag task #1 to the "In Progress" column
    And I drag task #1 to the "Not Started" column
    Then task #1 should be "Not Started"
