﻿using System;
using MattTools.Data;
namespace MattTools.Services;

public interface IRossumService
{
	Task<RossumData.LoginRespone> Login(RossumData.LoginForm form);
}
