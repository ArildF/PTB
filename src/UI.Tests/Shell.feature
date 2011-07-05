Feature: Shell
	In order to see about the open taskboard
	As a Windows user
	I want a top level window

Scenario: Show taskboard title
	Given that the database "C:\foo\bar.taskboard" already exists
	And that I open "C:\foo\bar.taskboard"
	Then the window title should be "bar - Personal Task Board"
