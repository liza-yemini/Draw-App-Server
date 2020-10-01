﻿using System;
using System.Text;
using Contracts;
using DrawPicContracts.DTO;
using DrawPicContracts.Interface;
using InfraContracts.DTO;

namespace CreateMarkerService
{
    [Register(Policy.Transient, typeof(ICreateMarkerService))]
    public class CreateMarkerServiceImpl : ICreateMarkerService
    {
        private readonly IMarkerDal _dal;
        public CreateMarkerServiceImpl(IMarkerDal dal)
        {
            _dal = dal;
        }
        public Response CreateMarker(CreateMarkerRequest request)
        {
            try
            {
                request.MarkerId = GetId(request.DocId);
                _dal.CreateMarker(request);
                CreateMarkerResponseOk ret = new CreateMarkerResponseOk
                {
                    //MarkerDto = request.MarkerDto
                    DocId = request.DocId,
                    BackColor = request.BackColor,
                    ForColor = request.ForColor,
                    Height = request.Height,
                    Width = request.Width,
                    LocationY = request.LocationY,
                    LocationX = request.LocationX,
                    MarkerId = request.MarkerId,
                    MarkerType = request.MarkerType,
                    UserId = request.UserId
                };
                return ret;
            }
            catch (Exception ex)
            {
                return new AppResponseError(ex.Message);
            }
        }

        private string GetId(string input)
        {
            var md5 = System.Security.Cryptography.MD5.Create();
            var ticks = DateTime.Now.Ticks;
            //User+ticks 
            var bytes = System.Text.Encoding.ASCII.GetBytes(input + ticks);
            var hashBytes = md5.ComputeHash(bytes);
            //var output = System.Text.Encoding.UTF8.GetString(hashBytes);
            StringBuilder sb = new StringBuilder();
            foreach (var t in hashBytes)
            {
                sb.Append(t.ToString("X2"));
            }

            return sb.ToString();
        }
    }
}
