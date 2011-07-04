Feature: Links
	In order to relate various tasks
	As a user
	I want to link tasks together

Scenario: Setting relative priority
	Given that the following tasks already exist and are loaded:
	|Title|
	|One|
	|Two|
	|Three|
	When I set task "Three" to be more important than task "One"
	Then the tasks should be in this order:
	|Title|
	|Three|
	|One|
	|Two|

Scenario: Relative priority is persisted
	Given that the following tasks already exist and are loaded:
	|Title|
	|One|
	|Two|
	|Three|
	When I set task "Three" to be more important than task "One"
	And I reload the taskboard
	Then the tasks should be in this order:
	|Title|
	|Three|
	|One|
	|Two|

Scenario: One task should not be both more and less important than another
	Given that the following tasks already exist and are loaded:
	| Title |
	| One   |
	| Two   |
	| Three |
	When I set task "Three" to be more important than task "One"
	And I set task "One" to be more important than task "Three"
	Then task "One" should be more important than task "Three"
	And task "Three" should not be more important than task "One"


Scenario: Importance is reflexive
	Given that the following tasks already exist and are loaded:
	| Title |
	| One   |
	| Two   |
	| Three |
	When I set task "Three" to be more important than task "One"
	Then task "Three" should be more important than task "One"
	And task "One" should be less important than task "Three"


