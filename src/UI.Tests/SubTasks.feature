Feature: Subtasks
	In order to better organize my tasks
	As a person
	I want to have subtasks of my tasks

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

Scenario: Subtasks of subtasks
	Given that the following tasks already exist and are loaded:
	|Title      |
	|Yo         |
	When I select task 'Yo'
	And click new subtask
	And click new subtask
	Then a new task should be created
	And the new task should be in edit mode
	And the new task should be in position #3
	And the new task should be indented 2 places
	And the new task should be selected

Scenario: Starting a subtask should start the parent
	Given that the following tasks already exist and are loaded:
    |Title      |
    |Do stuff         |
	And that task #1 has a subtask "Sub a"
	When I drag task #2 to the "In Progress" column
	Then task #1 should be "In Progress"


Scenario: Subtasks are sorted with their parents
	Given that the following tasks already exist and are loaded:
	| Title |
	| One   |
	| Two   |
	| Three |	
	When I set task "Three" to be more important than task "One"
	And I add a subtask "Three-A" to task "Three"
	Then the tasks should be in this order:
	| Title   |
	| Three   |
	| Three-A |
	|One|
	|Two|

Scenario: Should not be able to set a subtask more important than its parent
	Given that the following tasks already exist and are loaded:
	| Title |
	| One   |
	| Two   |
	| Three |
	When I add a subtask "One-A" to task "One"
	And I set task "One-A" to be more important than task "One"
	Then the tasks should be in this order:
	| Title   |
	| One   |
	| One-A   |
	| Two   |
	| Three |

Scenario: Should not be able to set a subtask more important than a task that is transitively more important
	Given that the following tasks already exist and are loaded:
	| Title |
	| One   |
	| Two   |
	| Three |
	When I set task "Three" to be more important than task "Two"
	And I set task "Two" to be more important than task "One"
	And I set task "One" to be more important than task "Three"
	Then the tasks should be in this order:
	| Title   |
	| Three	  |
	| Two   |
	| One |

