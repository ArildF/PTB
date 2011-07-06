Feature: Tasks
	In order to manage my time
	As a person
	I want to view and select tasks

Background:
	Given that the following tasks already exist and are loaded:
    | Title |
    | Yo    |
    | Oy    |
    | Toy   |

Scenario: Select a task
	When I select task #1
	Then task #1 should be selected

Scenario: Selecting a task unselects previously selected
	When I select task #1
	And I select task #2
	Then task #2 should be selected
	And task #1 should not be selected

Scenario: Editing a task selects it
	When I select task #1
	And I begin editing task #2
	Then task #2 should be selected
	And task #2 should be in edit mode


Scenario: Deselect all
	When I select task #1
	And I select task #2
	And I deselect all
	Then task #2 should not be selected
	And task #1 should not be selected
	And task #3 should not be selected
