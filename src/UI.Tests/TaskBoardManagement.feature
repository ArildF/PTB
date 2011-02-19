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
    Given that I enter "C:\foo\bar.taskboard" in the open taskboard dialog
    When I open a taskboard
    Then a taskboard should be loaded from "C:\foo\bar.taskboard"
    And a new taskboard should be loaded

Scenario: Remember the last opened taskboards
    Given that I open "C:\foo\bar.taskboard"
    And that I open "C:\bar\foo.taskboard"
    And that I open "C:\barbar\foofoo.taskboard"
    Then the dropdown for the open button should display these in this order:
    |Path|
    |C:\bar\foo.taskboard|
    |C:\foo\bar.taskboard|

Scenario: No remembered open taskboards
    Then the dropdown for the open button should contain no items

Scenario: Export tasks
    Given that the following tasks already exist and are loaded:
    |Title |
    |Foo    |
    |Bar    |
    |Baz|
    And that everything is saved
    And that I enter "C:\foo\bar.taskboard" in the export taskboard dialog
    When I click export task
    Then the tasks should be exported to a "C:\foo\bar.taskboard"
    And the exported tasks should contain 3 tasks
    And task #2 in the exported tasks should have the title "Bar"
    And the tasks should not have empty IDs