using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemDashServer.Application.Products.Commands;

public record DeleteProductCommand(int Id) : IRequest<bool>;
