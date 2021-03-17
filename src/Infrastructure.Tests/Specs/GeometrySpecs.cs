using System.Windows;
using System.Windows.Media;
using Machine.Specifications;

namespace Rogue.Ptb.Infrastructure.Tests.Specs
{
	[Subject(typeof (BuildPath))]
	public class When_constructing_a_path_geometry_through_the_fluent_API
	{
		Establish context = () =>
			{
				builder = BuildPath.From(0, 0).LineTo(10, 10).CurveTo(15, 15, SweepDirection.Clockwise);
			};



		Because of = () => { geometry = builder.Build(); };

		It should_have_one_figure = () => geometry.Figures.Count.ShouldEqual(1);

		It should_start_at_zero_zero = () => geometry.Figures[0].StartPoint.ShouldEqual(new Point(0, 0));

		It should_have_two_segments = () => geometry.Figures[0].Segments.Count.ShouldEqual(2);

		It should_have_first_a_line_segment = () => geometry.Figures[0].Segments[0].ShouldBeOfExactType<LineSegment>();

		It should_have_a_line_segment_to_ten_ten = () => ((LineSegment)geometry.Figures[0].Segments[0]).Point.ShouldEqual(new Point(10, 10));

		It should_have_second_an_arc_segment = () => geometry.Figures[0].Segments[1].ShouldBeOfExactType<ArcSegment>();

		It should_have_an_arc_segment_to_15_15 = () => ((ArcSegment)geometry.Figures[0].Segments[1]).Point.ShouldEqual(new Point(15, 15));


		private static PathGeometry geometry;
		private static BuildPath.IPathBuilder builder;
	}

	[Subject(typeof (BuildPath))]
	public class When_constructing_a_path_geometry_through_the_fluent_API_with_two_figures
	{
		Establish context = () => builder = BuildPath.From(0, 0).LineTo(10, 10).NewFigureFrom(0, 10).LineTo(10, 0);
		Because of = () => geometry = builder.Build();

		It should_have_two_figures = () => geometry.Figures.Count.ShouldEqual(2);

		private static BuildPath.IPathBuilder builder;
		private static PathGeometry geometry;
	}
}