using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemDashServer.Application.Products.Queries;
public record GetProductsQuery() : IRequest<IEnumerable<ProductDto>>;
