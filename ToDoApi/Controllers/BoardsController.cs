using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.EntityFrameworkCore;
using ToDoApi.Data;
using ToDoApi.Models;

namespace ToDoApi.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class BoardsController : ControllerBase
    {
        private readonly BoardsContext _boardsContext;
        public BoardsController(BoardsContext boardsContext)
        {
            _boardsContext = boardsContext;

            if (_boardsContext.Boards.Count() == 0)

            {
                Board board = new Board { Name = "Board1" };
                List<ToDo> ToDoList = new List<ToDo> { new ToDo { Title = "first", Done = false,Created=DateTime.Now,Updated=DateTime.Now,Board=board,BoardId=board.Id},
                 new ToDo {Title = "second", Done = false,Created=DateTime.Now,Updated=DateTime.Now,Board=board,BoardId=board.Id},
                 new ToDo {Title = "third", Done = false,Created=DateTime.Now,Updated=DateTime.Now,Board=board,BoardId=board.Id }};
                board.ToDoList = ToDoList;

                _boardsContext.Boards.Add(board);
                _boardsContext.SaveChanges();
            }
        }
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var boards = await _boardsContext.Boards.ToListAsync();
            return Ok(boards);
        }




        [HttpPost(Name = "AddBoard")]
        public IActionResult Create([FromBody] Board item)
        {
            if (item == null)
            {
                return BadRequest();
            }
            _boardsContext.Database.OpenConnection();
            try
            {
                _boardsContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.tblBoard ON");
                _boardsContext.Boards.Add(item);
                _boardsContext.SaveChanges();
                _boardsContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.tblBoard OFF");

            }
            finally
            {
                _boardsContext.Database.CloseConnection();

            }

            return CreatedAtRoute("GetBoard", new { id = item.Id }, item);
        }
        [HttpGet("{board_id}", Name = "GetBoard")]
        public IActionResult GetBoard(long board_id)
        {
            var item = _boardsContext.Boards.FirstOrDefault(t => t.Id == board_id);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }

        [HttpGet("Get-Task/{task_id}", Name = "GetTask")]
        public IActionResult GetTask(long task_id)
        {
            var item = _boardsContext.ToDos.FirstOrDefault(t => t.Id == task_id);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }

        [HttpPut("Change-Board-Title/{board_id}")]
        public IActionResult UpdateBoardTitle(long board_id , [FromBody] string title)

        {
            if (title == null)
            {
                return BadRequest();
            }

            var item = _boardsContext.Boards.FirstOrDefault(t => t.Id == board_id);
            if (item == null)
            {
                return NotFound(title);
            }
            item.Name = title;
            _boardsContext.Update(item);
            _boardsContext.SaveChanges();

            return new NoContentResult();



        }

        [HttpPost("Add-To-Do/{board_id}")]

        public IActionResult AddToDo(long board_id, [FromBody] ToDo task)
        {
           var item=_boardsContext.Boards.FirstOrDefault(t => t.Id == board_id);
            if (item == null)
            {
                return NotFound("No Board with this Id exist");
            }
            task.BoardId =(int)board_id;
            _boardsContext.ToDos.Add(task);
            _boardsContext.SaveChanges();

            return CreatedAtRoute("GetTask", new { id = task.Id }, task);

        }

        [HttpPost("Task/Change-Title/{task_id}")]

        public IActionResult UpdateTaskTitle(long task_id,[FromBody] string title) {

            if (title == null)
            {
                return BadRequest();
            }

            var item = _boardsContext.ToDos.FirstOrDefault(t => t.Id == task_id);
            if (item == null)
            {
                return NotFound(title);
            }
            item.Title = title;
            _boardsContext.Update(item);
            _boardsContext.SaveChanges();

            return new NoContentResult();


        }

        [HttpPost("Task/Change-Status/{task_id}")]

        public IActionResult UpdateTaskStatus(long task_id, [FromBody] bool status)
        {

            var item = _boardsContext.ToDos.FirstOrDefault(t => t.Id == task_id);
            if (item == null)
            {
                return NotFound("Task with this Id does not exists");
            }
            item.Done = status;
            _boardsContext.Update(item);
            _boardsContext.SaveChanges();

            return new NoContentResult();


        }

        [HttpGet("Tasks/{board_id}")]

        public async Task<IActionResult> GetTasksOnBoard(long board_id)
        {

            var tasks = await _boardsContext.ToDos.Where(item => item.BoardId == board_id).ToListAsync();
            if (tasks == null)
            {
                return NotFound("no board exist");
            }
            return Ok(tasks);


        }

        [HttpGet("Uncompleted_Tasks/{board_id}")]

        public async Task<IActionResult> GetTasksOnBoardNotCompleted(long board_id)
        {

            var tasks = await _boardsContext.ToDos.Where(item => item.BoardId == board_id && item.Done==true).ToListAsync();
            if (tasks == null)
            {
                return NotFound("no board exist");
            }
            return Ok(tasks);


        }


        [HttpDelete("{board_id}")]
        public IActionResult DeleteBoard(long board_id)
        {
            var board=_boardsContext.Boards.FirstOrDefault(t => t.Id == board_id);
            if (board ==null)
            {
                return NotFound();

            }
            _boardsContext.Boards.Remove(board);
            _boardsContext.SaveChanges();
            return new NoContentResult();
        }

        [HttpDelete("Tasks/{task_id}")]
        public IActionResult DeleteTask(long task_id)
        {
            var task = _boardsContext.ToDos.FirstOrDefault(t => t.Id == task_id);
            if (task == null)
            {
                return NotFound();

            }
            _boardsContext.ToDos.Remove(task);
            _boardsContext.SaveChanges();
            return new NoContentResult();
        }







    }
}
