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
