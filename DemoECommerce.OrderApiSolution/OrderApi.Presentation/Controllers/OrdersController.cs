﻿using eCommerce.SharedLibrary.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderApi.Application.DTOs;
using OrderApi.Application.DTOs.Conversions;
using OrderApi.Application.Interfaces;
using OrderApi.Application.Services;

namespace OrderApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController(IOrder orderInterface, IOrderService orderService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetOrders()
        {
            var orders = await orderInterface.GetAllAsync();
            if (!orders.Any())
            {
                return NotFound("No order detected in the database");
            }
            var (_, list) = OrderConversion.FromEntity(null, orders);
            return !list!.Any() ? NotFound() : Ok(list);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<OrderDTO>> GetOrder(int id)
        {
            var order = await orderInterface.GetByIdAsync(id);
            if (order is null)
            {
                return NotFound();
            }
            var (_order, _) = OrderConversion.FromEntity(order, null!);
            return Ok(_order);
        }
        [HttpGet("client/{clientId:int}")]
        public async Task<ActionResult<OrderDTO>> GetClientOrder(int clientId)
        {
            if (clientId <= 0)
            {
                return BadRequest("Invalid data provided");
            }
            var orders = await orderService.GetOrdersByClientId(clientId);
            return !orders.Any() ? NotFound() : Ok(orders);
        }

        [HttpGet("details/{orderId:int}")]
        public async Task<ActionResult<OrderDetailsDTO>> GetOrderDetails(int orderId)
        {
            if (orderId <= 0) return BadRequest("Invalid data provided");
            var orderDetail = await orderService.GetOrderDetails(orderId);
            return orderDetail.OrderId > 0 ? Ok(orderDetail) : NotFound("No order found");
        }


        [HttpPost]
        public async Task<ActionResult<Response>> CreateOrder(OrderDTO orderDTO)
        {
            // check model state if all data annotation are passed
            if (!ModelState.IsValid)
            {
                return BadRequest("Incomplete data submitted");
            }

            // convert to entity
            var getEntity = OrderConversion.ToEntity(orderDTO);
            var response = await orderInterface.CreateAsync(getEntity);
            return response.Flag ? Ok(response) : BadRequest(response);
        }

        [HttpPut]
        public async Task<ActionResult<Response>> UpdateOrder(OrderDTO orderDTO)
        {
            // check model state is all data annotation are passed
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // convert to entity
            var order = OrderConversion.ToEntity(orderDTO);
            var response = await orderInterface.UpdateAsync(order);

            return response.Flag ? Ok(response) : BadRequest(response);
        }

        [HttpDelete]
        public async Task<ActionResult<Response>> DeleteOrder(OrderDTO orderDTO)
        {
            // convert to entity
            var order = OrderConversion.ToEntity(orderDTO);
            var response = await orderInterface.DeleteAsync(order);

            return response.Flag ? Ok(response) : BadRequest(response);
        }
    }
}
