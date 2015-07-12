#!/usr/bin/env node

var spawn = require('child_process').spawn;

process.stdin.setEncoding('ascii');

function startServer() {
  var args = ['--watch', '.', 'kestrel'];
  
  var opts = { stdio: ['pipe', 1, 2] };
  
  return spawn('dnx', args, opts);
}

function loop() {
  console.log('Starting...');
  
  var server = startServer();
  var closed = false;
  
  server.on('close', handleServerClose);
  process.on('SIGINT', handleSIGINT);
  process.stdin.on('data', handleInput);
  process.on('exit', handleExit);
  
  function sendStop() {
    if (closed) return;
    
    server.stdin.write('\n', 'ascii');
  }
  
  function forceStop() {
    sendStop();
    server.kill();
  }
  
  function handleServerClose(code) {
    closed = true;
    
    if (code === 0) {
      console.log('-- WatchEvent');
      
      setTimeout(loop, 1000);
    } else {
      process.exit(code);
    }
    
    process.removeListener('SIGINT', handleSIGINT);
    process.stdin.removeListener('data', handleInput);
  }
  
  function handleSIGINT() {
    console.log('Exiting...');
    forceStop();
    process.exit(0);
  }
  
  function handleInput(data) {
    if (data === '\n') {
      console.log('Restarting...');
      sendStop();
    }
  }
  
  function handleExit() {
    forceStop();
  }
}

loop();
