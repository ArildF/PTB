Feature: Export/import
	In order to retain my taskboards through upgrades
	As a user
	I want to export my tasks to XML and import them back


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
    And the exported tasks should not have empty IDs


Scenario: Import tasks
    Given an export file containing these tasks at "C:\foo\export.xml"
    |Title |
    |Sidious    |
    |Tyranus    |
    |Malak|
    And I create a new taskboard
    And that the following tasks already exist and are loaded:
    |Title|
    |Caedus|
    |Plagueis|
    And that I enter "C:\foo\export.xml" in the import taskboard dialog
    When I click import tasks
    Then the taskboard should contain these tasks:
    |Title|
    |Sidious|
    |Tyranus|
    |Malak|
    |Caedus|
    |Plagueis|
    And the loaded tasks should not have empty IDs

Scenario: Export/import
    Given a varied set of tasks loaded into the taskboard
    And that I enter "C:\foo\importexport.xml" in the export taskboard dialog
    And that I enter "C:\foo\importexport.xml" in the import taskboard dialog
    And that I enter "C:\foo\taskboard.taskboard" in the create taskboard dialog
    When I click export task
    And I create a new taskboard
    And I click import tasks
    Then the loaded tasks should have the same attributes as the original set of tasks