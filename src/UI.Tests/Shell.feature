Feature: Shell
	In order to see about the open taskboard
	As a Windows user
	I want a top level window

Scenario: Show taskboard title
	Given that the database "C:\foo\bar.taskboard" already exists
	And that I open "C:\foo\bar.taskboard"
	Then the window title should be "bar - Personal Task Board"

Scenario: Pass in taskboard on command line
	Given that the database "C:\foo\bar.taskboard" already exists
	And that I pass in "C:\foo\bar.taskboard" on the command line
	When I start the application
	Then a taskboard should be loaded from "C:\foo\bar.taskboard"