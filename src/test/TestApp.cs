using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace Test
{
	public class TestApp
	{
		[Fact]
		public void Test1()
		{
			double[,] adjacencyMatrix = { {1, 1, 1}, { 1, 1, 1 }, { 1, 1, 1 } };
			Assert.True(App.Program.IsMatrixValid(adjacencyMatrix));
		}

		[Fact]
		public void Test2()
		{
			int vertex = 3;
			int size = 2;

			Assert.True(App.Program.IsVertexValid(vertex, size));
		}
	}
}