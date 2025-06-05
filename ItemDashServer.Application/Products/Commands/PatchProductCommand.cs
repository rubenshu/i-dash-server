using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ItemDashServer.Application.Products.Commands;

public record PatchProductCommand(int Id, JsonDocument PatchDoc) : IRequest<bool>;
