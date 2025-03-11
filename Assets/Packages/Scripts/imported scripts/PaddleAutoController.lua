--[[
	Class to control a paddle without a player
	It tries to move the paddle to where the ball is
	It needs a Paddle to controll
	It needs a Ball to track
	
	Error

main.lua:99: attempt to call global 'PaddleAutoController' (a nil value)


Traceback

main.lua:99: in function 'load'
[C]: in function 'xpcall'
[C]: in function 'xpcall'
]]
PaddleAutoController = Class{}

--~ function PaddleAutoController:init()
function PaddleAutoController:init(ball, paddle, paddleSpeed)
	self.ball = ball
	self.paddle = paddle
	self.paddleSpeed = paddleSpeed
end

function PaddleAutoController:update(dt)
	if self:isPaddleAboveBall() then
		self.paddle.dy = self.paddleSpeed
	elseif self:isPaddleBelowBall() then
		self.paddle.dy = -self.paddleSpeed
	else
		self.paddle.dy = 0
	end
end

function PaddleAutoController:isPaddleAboveBall()
	return self.paddle.y + self.paddle.height < self.ball.y
end

function PaddleAutoController:isPaddleBelowBall()
	return self.paddle.y > self.ball.y + self.ball.height
end
