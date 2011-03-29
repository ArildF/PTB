Feature: Create taskboard
	In order to organize my tasks
	As an enterprising person
	I want to create and open taskboards

Scenario: Create new taskboard
	Given that I enter "C:\foo\bar.taskboard" in the create taskboard dialog
    When I create a new taskboard
    Then a new taskboard database should be created in "C:\foo\bar.taskboard"
    And a new taskboard should be loaded

Scenario: Open existing taskboard
	Given that the database "C:\foo\bar.taskboard" already exists
    And that I enter "C:\foo\bar.taskboard" in the open taskboard dialog
    When I open a taskboard
    Then a taskboard should be loaded from "C:\foo\bar.taskboard"
    And a new taskboard should be loaded

Scenario: Remember the last opened taskboards
	Given that the following taskboards already exist:
	|Path|
    |C:\bar\foo.taskboard|
    |C:\foo\bar.taskboard|
	|C:\barbar\foofoo.taskboard|
    And that I open "C:\foo\bar.taskboard"
    And that I open "C:\bar\foo.taskboard"
    And that I open "C:\barbar\foofoo.taskboard"
    Then the dropdown for the open button should display these in this order:
    |Path|
    |C:\bar\foo.taskboard|
    |C:\foo\bar.taskboard|

Scenario: No remembered open taskboards
    Then the dropdown for the open button should contain no items

