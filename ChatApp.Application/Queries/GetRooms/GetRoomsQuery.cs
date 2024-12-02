using System;
using ChatApp.Domain.Entities;
using MediatR;

namespace ChatApp.Application.Queries.GetRooms;

public class GetRoomsQuery : IRequest<IEnumerable<ChatRoom>>
{

}
