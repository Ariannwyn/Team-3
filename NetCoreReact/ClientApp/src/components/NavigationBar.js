﻿import React, { useState, useEffect } from "react";
import { makeStyles } from "@material-ui/core/styles";
import AppBar from "@material-ui/core/AppBar";
import Toolbar from "@material-ui/core/Toolbar";
import Button from "@material-ui/core/Button";
import IconButton from "@material-ui/core/IconButton";
import MenuIcon from "@material-ui/icons/Menu";
import clsx from "clsx";
import Logo from "./Logo";
import Login from "./Login";
import useAuth from "../hooks/useAuth";
import Cookies from "js-cookie";
import { Avatar, Box, Menu, MenuItem } from "@material-ui/core";
import DropDownArrow from "@material-ui/icons/ArrowDropDown";

const drawerWidth = 240;

const useStyles = makeStyles(theme => ({
  root: {
    flexGrow: 1
  },
  grow: {
    flexGrow: 1
  },
  menuButton: {
    marginRight: theme.spacing(2)
  },
  appBarShift: {
    width: `calc(100% - ${drawerWidth}px)`,
    marginLeft: drawerWidth,
    transition: theme.transitions.create(["margin", "width"], {
      easing: theme.transitions.easing.easeOut,
      duration: theme.transitions.duration.enteringScreen
    })
  },
  inputRoot: {
    color: "inherit"
  },
  inputInput: {
    padding: theme.spacing(1, 1, 1, 7),
    transition: theme.transitions.create("width"),
    width: "100%",
    [theme.breakpoints.up("sm")]: {
      width: 120,
      "&:focus": {
        width: 200
      }
    }
  },
  button: {
    marginLeft: theme.spacing(1),
	marginRight: theme.spacing(1)
  }
}));

export default function NavigationBar({ handleDrawerOpen, open }) {
  const classes = useStyles();
  const [loginOpen, setLoginOpen] = useState(false);
  const { logout, authState } = useAuth();
  const [avatarUrl, setAvatarUrl] = useState("");
  const [anchorEl, setAnchorEl] = React.useState(null);

  useEffect(() => {
    let avatar = Cookies.get("Avatar-Url");
    if (avatar) setAvatarUrl(avatar);
    else setAvatarUrl("");
  }, [authState]);

  const handleClose = () => {
    setAnchorEl(null);
  };

  const handleClick = event => {
    setAnchorEl(event.currentTarget);
  };

  const handleLogout = () => {
    logout();
    handleClose();
  };

  return (
    <div className={classes.root}>
      <AppBar
        position="fixed"
        className={clsx(classes.appBar, {
          [classes.appBarShift]: open
        })}
      >
        <Toolbar>
          {!open && (
            <IconButton
              edge="start"
              className={classes.menuButton}
              color="inherit"
              aria-label="Menu"
              onClick={handleDrawerOpen}
            >
              <MenuIcon />
            </IconButton>
          )}
          {!open && <Logo />}
          <div className={classes.grow} />
          {authState.isAuthenticated ? (
            <>
              <Box display="flex" alignItems="center" onClick={handleClick}>
                <Avatar
                  alt="Profile"
                  src={avatarUrl}
                  className={classes.avatar}
                />
                <DropDownArrow />
              </Box>
              <Menu
                id="simple-menu"
                anchorEl={anchorEl}
                keepMounted
                open={Boolean(anchorEl)}
                onClose={handleClose}
                getContentAnchorEl={null}
                anchorOrigin={{
                  vertical: "bottom",
                  horizontal: "center"
                }}
                transformOrigin={{
                  vertical: "top",
                  horizontal: "center"
                }}
              >
                <MenuItem onClick={handleLogout}>Logout</MenuItem>
              </Menu>
            </>
          ) : (
            <Button
              color="inherit"
              className={classes.button}
              onClick={() => setLoginOpen(true)}
            >
              Login
            </Button>
          )}
        </Toolbar>
      </AppBar>
      <Login open={loginOpen} close={() => setLoginOpen(false)} />
    </div>
  );
}
