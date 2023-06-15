﻿using System.Collections.Generic;
using Hospital.Core.PhysicalAssets.Models;
using Hospital.Serialization;

namespace Hospital.Core.PhysicalAssets.Repositories;

public class EquipmentOrderRepository
{
    private const string FilePath = "../../../Data/equipmentOrders.csv";
    private const string OrderItemFilePath = "../../../Data/equipmentOrderItems.csv";

    private static EquipmentOrderRepository? _instance;
    private List<EquipmentOrder>? _orders;

    private EquipmentOrderRepository()
    {
    }

    public static EquipmentOrderRepository Instance
    {
        get { return _instance ??= new EquipmentOrderRepository(); }
    }

    public void AddItemsFromCsv(List<EquipmentOrder> orders)
    {
        var allEquipmentOrderItems = CsvSerializer<EquipmentOrderItem>.FromCSV(OrderItemFilePath);

        foreach (var orderItem in allEquipmentOrderItems)
        {
            var order = orders.Find(order => order.Id == orderItem.OrderId);
            if (order != null) order.Items.Add(orderItem);
        }
    }

    public List<EquipmentOrder> GetAll()
    {
        if (_orders == null)
        {
            _orders = CsvSerializer<EquipmentOrder>.FromCSV(FilePath);
            AddItemsFromCsv(_orders);
        }

        return _orders;
    }

    public void Add(EquipmentOrder order)
    {
        var orders = GetAll();
        orders.Add(order);
        WriteOrderItemsFromOrdersToCsv(orders);
        CsvSerializer<EquipmentOrder>.ToCSV(orders, FilePath);
    }

    private void WriteOrderItemsFromOrdersToCsv(List<EquipmentOrder> orders)
    {
        var orderItems = new List<EquipmentOrderItem>();


        foreach (var orderInList in orders) orderItems.AddRange(orderInList.Items);

        CsvSerializer<EquipmentOrderItem>.ToCSV(orderItems, OrderItemFilePath);
    }

    public void Update(EquipmentOrder order)
    {
        var orders = GetAll();
        var orderToUpdate = orders.Find(o => o.Id == order.Id);
        if (orderToUpdate == null) throw new KeyNotFoundException();

        orderToUpdate.DeliveryDateTime = order.DeliveryDateTime;
        orderToUpdate.Items = order.Items;
        CsvSerializer<EquipmentOrder>.ToCSV(orders, FilePath);
        WriteOrderItemsFromOrdersToCsv(orders);
    }

    public void DeleteAll()
    {
        if (_orders == null) return;
        _orders.Clear();
        CsvSerializer<EquipmentOrder>.ToCSV(_orders, FilePath);
        WriteOrderItemsFromOrdersToCsv(_orders);
        _orders = null;
    }
}